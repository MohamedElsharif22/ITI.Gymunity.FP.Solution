using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ITI.Gymunity.FP.Application.Services
{
    public interface IChatBotService
    {
        Task<string> AskAboutPackagesAsync(string question);
        Task<string> AskAsync(string prompt, string? modelName = null, bool requireJsonOnly = false);
    }

    public class ChatBotService : IChatBotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly ILogger<ChatBotService> _logger;

        public ChatBotService(
            IUnitOfWork unitOfWork,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<ChatBotService> logger)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["OpenRouter:ApiKey"] ?? configuration["HuggingFace:ApiKey"]
                      ?? throw new InvalidOperationException("API key is not configured");
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                _logger.LogWarning("No API key configured for ChatBotService (OpenRouter/HuggingFace)");
            }
        }

        public async Task<string> AskAboutPackagesAsync(string question)
        {
            try
            {
                var packageRepo = _unitOfWork.Repository<Package, IPackageRepository>();
                var packages = await packageRepo.GetAllActiveWithProgramsAsync(); // Use only one call

                var packagesContext = FormatPackagesForAI(packages);

                var prompt = $@"أنت مساعد ذكي متخصص في الإجابة عن أسئلة حول الباكدجات المتاحة في منصة Gymunity.

معلومات الباكدجات المتاحة:
{packagesContext}

السؤال: {question}

يرجى الإجابة بالعربية بشكل واضح ومفيد. إذا كان السؤال عن الأسعار، اذكر الأسعار بالتفصيل. إذا كان السؤال عن المدربين، اذكر أسماء المدربين. إذا كان السؤال عن الباكدجات السنوية، اذكر الباكدجات التي لها سعر سنوي.";

                var model = "tngtech/deepseek-r1t2-chimera:free";
                return await AskAsync(prompt, modelName: model, requireJsonOnly: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing AskAboutPackages prompt");
                return "عذراً، حدث خطأ أثناء معالجة السؤال.";
            }
        }

        public async Task<string> AskAsync(string prompt, string? modelName = null, bool requireJsonOnly = false)
        {
            try
            {
                var packageRepo = _unitOfWork.Repository<Package, IPackageRepository>();
                var packages = await packageRepo.GetAllActiveWithProgramsAsync(); // One call only

                string? systemPrefix = null;
                if (requireJsonOnly)
                {
                    systemPrefix = "You are an API that MUST return only valid JSON and nothing else. No explanations, no markdown, no extra text. Return {} if you cannot comply.";
                }

                var messages = systemPrefix != null
                    ? new object[] { new { role = "system", content = systemPrefix }, new { role = "user", content = prompt } }
                    : new object[] { new { role = "user", content = prompt } };

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri("https://openrouter.ai/api/v1/");
                httpClient.DefaultRequestHeaders.Remove("Authorization");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var effectiveModel = modelName ?? "tngtech/deepseek-r1t2-chimera:free";

                var payload = new
                {
                    model = effectiveModel,
                    messages = messages,
                    stream = false
                };

                var requestJson = JsonSerializer.Serialize(payload);
                using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
                {
                    Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
                };

                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("OpenRouter API error: {Status} - {Content}", response.StatusCode, content);
                    return GenerateFallbackResponse(prompt, packages);
                }

                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("choices", out var choices) && choices.ValueKind == JsonValueKind.Array && choices.GetArrayLength() > 0)
                    {
                        var first = choices[0];
                        if (first.TryGetProperty("message", out var message) && message.TryGetProperty("content", out var msgContent))
                        {
                            var text = msgContent.GetString();
                            if (!string.IsNullOrWhiteSpace(text)) return text.Trim();
                        }
                        if (first.TryGetProperty("text", out var textProp))
                        {
                            var text = textProp.GetString();
                            if (!string.IsNullOrWhiteSpace(text)) return text.Trim();
                        }
                    }

                    if (root.TryGetProperty("message", out var messageObj) && messageObj.TryGetProperty("content", out var contentProp))
                    {
                        var text = contentProp.GetString();
                        if (!string.IsNullOrWhiteSpace(text)) return text.Trim();
                    }
                }
                catch (JsonException)
                {
                    _logger.LogWarning("Could not parse OpenRouter response as JSON, returning raw content");
                }

                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenRouter API");
                var packageRepo = _unitOfWork.Repository<Package, IPackageRepository>();
                var packages = await packageRepo.GetAllActiveWithProgramsAsync();
                return GenerateFallbackResponse(prompt, packages);
            }
        }

        private string FormatPackagesForAI(IReadOnlyList<Package> packages)
        {
            if (packages == null || packages.Count == 0)
                return "لا توجد باكدجات متاحة حالياً.";

            var sb = new StringBuilder();

            foreach (var package in packages)
            {
                var trainer = package.Trainer;
                var user = trainer?.User;
                var trainerName = user?.FullName ?? trainer?.Handle ?? "غير محدد";

                sb.AppendLine($"- الباكدج: {package.Name}");
                sb.AppendLine($"  الوصف: {package.Description}");
                sb.AppendLine($"  السعر الشهري: {package.PriceMonthly} {package.Currency}");
                sb.AppendLine($"  السعر السنوي: {(package.PriceYearly.HasValue ? package.PriceYearly.Value.ToString() : "غير متاح")} {package.Currency}");

                sb.AppendLine($"  المدرب:");
                sb.AppendLine($"   - الاسم: {trainerName}");
                if (!string.IsNullOrWhiteSpace(user?.UserName)) sb.AppendLine($"   - اسم المستخدم: {user!.UserName}");
                if (!string.IsNullOrWhiteSpace(user?.Email)) sb.AppendLine($"   - البريد الإلكتروني: {user!.Email}");
                if (!string.IsNullOrWhiteSpace(trainer?.Handle)) sb.AppendLine($"   - الهاndl: {trainer.Handle}");
                if (trainer != null)
                {
                    sb.AppendLine($"   - سنوات الخبرة: {trainer.YearsExperience}");
                }

                sb.AppendLine();
            }

            // Add summary statistics
            var monthlyPrices = packages.Select(p => p.PriceMonthly).ToList();
            var yearlyPrices = packages.Where(p => p.PriceYearly.HasValue).Select(p => p.PriceYearly!.Value).ToList();

            sb.AppendLine("ملخص:");
            sb.AppendLine($"- إجمالي الباكدجات: {packages.Count}");

            if (monthlyPrices.Any())
            {
                sb.AppendLine($"- السعر الشهري يبدأ من: {monthlyPrices.Min()} {packages.First().Currency}");
                sb.AppendLine($"- السعر الشهري يصل إلى: {monthlyPrices.Max()} {packages.First().Currency}");
            }

            if (yearlyPrices.Any())
            {
                sb.AppendLine($"- السعر السنوي يبدأ من: {yearlyPrices.Min()} {packages.First().Currency}");
                sb.AppendLine($"- السعر السنوي يصل إلى: {yearlyPrices.Max()} {packages.First().Currency}");
            }

            return sb.ToString();
        }

        private string GenerateFallbackResponse(string question, IReadOnlyList<Package> packages)
        {
            if (packages == null || packages.Count == 0)
                return "عذراً، لا توجد باكدجات متاحة حالياً.";

            var questionLower = question.ToLower();

            if (questionLower.Contains("سنوي") || questionLower.Contains("سنوية") || questionLower.Contains("annual"))
            {
                var annualPackages = packages.Where(p => p.PriceYearly.HasValue).ToList();
                if (annualPackages.Any())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("الباكدجات السنوية المتاحة:");
                    foreach (var pkg in annualPackages)
                    {
                        var trainerName = pkg.Trainer?.User?.FullName ?? "غير محدد";
                        sb.AppendLine($"- {pkg.Name} (المدرب: {trainerName}) - السعر السنوي: {pkg.PriceYearly} {pkg.Currency}");
                    }
                    return sb.ToString();
                }
                return "لا توجد باكدجات سنوية متاحة حالياً.";
            }

            if (questionLower.Contains("سعر") || questionLower.Contains("ثمن") || questionLower.Contains("price"))
            {
                var monthlyPrices = packages.Select(p => p.PriceMonthly).ToList();
                var yearlyPrices = packages.Where(p => p.PriceYearly.HasValue).Select(p => p.PriceYearly!.Value).ToList();

                var sb = new StringBuilder();
                sb.AppendLine("أسعار الباكدجات:");
                sb.AppendLine($"- السعر الشهري يبدأ من {monthlyPrices.Min()} {packages.First().Currency} إلى {monthlyPrices.Max()} {packages.First().Currency}");

                if (yearlyPrices.Any())
                    sb.AppendLine($"- السعر السنوي يبدأ من {yearlyPrices.Min()} {packages.First().Currency} إلى {yearlyPrices.Max()} {packages.First().Currency}");

                return sb.ToString();
            }

            if (questionLower.Contains("مدرب") || questionLower.Contains("trainer") || questionLower.Contains("اسم") || questionLower.Contains("ترنر"))
            {
                var sb = new StringBuilder();
                sb.AppendLine("قائمة الباكدجات والمدربين:");
                foreach (var pkg in packages)
                {
                    var trainer = pkg.Trainer;
                    var user = trainer?.User;
                    var trainerName = user?.FullName ?? trainer?.Handle ?? "غير محدد";
                    sb.AppendLine($"- {pkg.Name} (المدرب: {trainerName})");
                    if (!string.IsNullOrWhiteSpace(user?.Email)) sb.AppendLine($" البريد: {user!.Email}");
                    if (!string.IsNullOrWhiteSpace(trainer?.Handle)) sb.AppendLine($" Handle: {trainer.Handle}");
                }
                return sb.ToString();
            }

            var defaultSb = new StringBuilder();
            defaultSb.AppendLine($"لدينا {packages.Count} باكدج متاح:");
            foreach (var pkg in packages)
            {
                var trainerName = pkg.Trainer?.User?.FullName ?? pkg.Trainer?.Handle ?? "غير محدد";
                defaultSb.AppendLine($"- {pkg.Name} (المدرب: {trainerName}) - السعر الشهري: {pkg.PriceMonthly} {pkg.Currency}");
            }
            return defaultSb.ToString();
        }
    }
}
