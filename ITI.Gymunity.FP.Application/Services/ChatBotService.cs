using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

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
            // read OpenRouter key if present, otherwise fall back to HuggingFace key
            _apiKey = configuration["OpenRouter:ApiKey"] ?? configuration["HuggingFace:ApiKey"] ?? throw new InvalidOperationException("API key is not configured");
            _logger = logger;
        }

        public async Task<string> AskAboutPackagesAsync(string question)
        {
            try
            {
                // Get all packages with trainer information
                var packageRepo = _unitOfWork.Repository<Package, IPackageRepository>();
                var packages = await packageRepo.GetAllActiveWithTrainerUserAsync();

                // Format packages data for the AI
                var packagesContext = FormatPackagesForAI(packages);

                // Create the prompt
                var prompt = $@"أنت مساعد ذكي متخصص في الإجابة عن أسئلة حول الباكدجات المتاحة في منصة Gymunity.

معلومات الباكدجات المتاحة:
{packagesContext}

السؤال: {question}

يرجى الإجابة بالعربية بشكل واضح ومفيد. إذا كان السؤال عن الأسعار، اذكر الأسعار بالتفصيل. إذا كان السؤال عن المدربين، اذكر أسماء المدربين. إذا كان السؤال عن الباكدجات السنوية، اذكر الباكدجات التي لها سعر سنوي.";

                // Use DeepSeek model for general chat
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
                // Load packages for context if needed (kept for backward compatibility)
                var packageRepo = _unitOfWork.Repository<Package, IPackageRepository>();
                var packages = await packageRepo.GetAllActiveWithTrainerUserAsync();

                // Build system/user messages
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

                // Parse standard OpenRouter/OpenAI-like response
                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("choices", out var choices) && choices.ValueKind == JsonValueKind.Array && choices.GetArrayLength() >0)
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

                    // Some providers return a top-level 'message' object
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

                // Return raw content as last resort
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenRouter API");
                var packageRepo = _unitOfWork.Repository<Package, IPackageRepository>();
                var packages = await packageRepo.GetAllActiveWithTrainerUserAsync();
                return GenerateFallbackResponse(prompt, packages);
            }
        }

        private string FormatPackagesForAI(IReadOnlyList<Package> packages)
        {
            if (packages == null || packages.Count ==0)
            {
                return "لا توجد باكدجات متاحة حالياً.";
            }

            var sb = new StringBuilder();

            foreach (var package in packages)
            {
                var trainerName = package.Trainer?.User?.FullName ?? "غير محدد";
                var trainerHandle = package.Trainer?.Handle ?? "";

                sb.AppendLine($"- الباكدج: {package.Name}");
                sb.AppendLine($"  المدرب: {trainerName} ({trainerHandle})");
                sb.AppendLine($"  الوصف: {package.Description}");
                sb.AppendLine($"  السعر الشهري: {package.PriceMonthly} {package.Currency}");

                if (package.PriceYearly.HasValue)
                {
                    sb.AppendLine($"  السعر السنوي: {package.PriceYearly.Value} {package.Currency}");
                }
                else
                {
                    sb.AppendLine($"  السعر السنوي: غير متاح");
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
            if (packages == null || packages.Count ==0)
            {
                return "عذراً، لا توجد باكدجات متاحة حالياً.";
            }

            var questionLower = question.ToLower();

            // Handle annual packages question
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

            // Handle price range question
            if (questionLower.Contains("سعر") || questionLower.Contains("ثمن") || questionLower.Contains("price"))
            {
                var monthlyPrices = packages.Select(p => p.PriceMonthly).ToList();
                var yearlyPrices = packages.Where(p => p.PriceYearly.HasValue).Select(p => p.PriceYearly!.Value).ToList();

                var sb = new StringBuilder();
                sb.AppendLine("أسعار الباكدجات:");
                sb.AppendLine($"- السعر الشهري يبدأ من {monthlyPrices.Min()} {packages.First().Currency} إلى {monthlyPrices.Max()} {packages.First().Currency}");

                if (yearlyPrices.Any())
                {
                    sb.AppendLine($"- السعر السنوي يبدأ من {yearlyPrices.Min()} {packages.First().Currency} إلى {yearlyPrices.Max()} {packages.First().Currency}");
                }

                return sb.ToString();
            }

            // Handle trainer names question
            if (questionLower.Contains("مدرب") || questionLower.Contains("trainer"))
            {
                var trainers = packages
                    .Where(p => p.Trainer?.User != null)
                    .Select(p => p.Trainer!.User!.FullName)
                    .Distinct()
                    .ToList();

                if (trainers.Any())
                {
                    return "المدربون المتاحون: " + string.Join(", ", trainers);
                }
                return "لا توجد معلومات عن المدربين متاحة حالياً.";
            }

            // Default response - list all packages
            var defaultSb = new StringBuilder();
            defaultSb.AppendLine($"لدينا {packages.Count} باكدج متاح:");
            foreach (var pkg in packages)
            {
                var trainerName = pkg.Trainer?.User?.FullName ?? "غير محدد";
                defaultSb.AppendLine($"- {pkg.Name} (المدرب: {trainerName}) - السعر الشهري: {pkg.PriceMonthly} {pkg.Currency}");
            }
            return defaultSb.ToString();
        }
    }

    // Response models for OpenRouter/OpenAI-like API not needed as we parse JSON dynamically
}

