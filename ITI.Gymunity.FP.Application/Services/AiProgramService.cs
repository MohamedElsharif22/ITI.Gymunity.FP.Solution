using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Application.DTOs.Program;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ITI.Gymunity.FP.Application.Services
{
 public class AiProgramService : IAiProgramService
 {
 private readonly IHttpClientFactory _httpClientFactory;
 private readonly ILogger<AiProgramService> _logger;
 private readonly string _apiKey;

 public AiProgramService(IHttpClientFactory httpClientFactory, ILogger<AiProgramService> logger, IConfiguration configuration)
 {
 _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
 _logger = logger ?? throw new ArgumentNullException(nameof(logger));
 _apiKey = configuration?["OpenRouter:ApiKey"] ?? throw new InvalidOperationException("OpenRouter API key is not configured");
 }

 public async Task<ProgramAiResult> GenerateProgramAsync(ProgramAiCreateRequest request)
 {
 if (request == null) throw new ArgumentNullException(nameof(request));

 const string systemPrefix = "You are a Gym Program Generator that MUST return only valid JSON and nothing else. If you cannot comply return {}.";

 // User payload now contains only trainerProfileId and text (goal)
 var userPayload = JsonSerializer.Serialize(new { trainerProfileId = request.TrainerProfileId, text = request.Goal });

 var userPrompt = new StringBuilder();
 userPrompt.AppendLine("You are a Gym Program Generator AI.");
 userPrompt.AppendLine("User input (two parameters):");
 userPrompt.AppendLine(userPayload);
 userPrompt.AppendLine();
 userPrompt.AppendLine("Generate a complete program as JSON ONLY with fields: programName, goal, durationWeeks, daysPerWeek, exercisesPerDay, notes, schedule (weeks -> days -> exercises). Each exercise: name, sets, reps.");
 userPrompt.AppendLine("Do NOT include any text outside the JSON object. If unknown, use null.");

 var client = _httpClientFactory.CreateClient();
 client.Timeout = TimeSpan.FromMinutes(2);
 client.BaseAddress = new Uri("https://openrouter.ai/api/v1/");
 client.DefaultRequestHeaders.Remove("Authorization");
 client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

 var model = "openai/gpt-oss-20b:free";
 var maxAttempts =3;
 string lastRaw = string.Empty;

 for (int attempt =1; attempt <= maxAttempts; attempt++)
 {
 var messages = new object[]
 {
 new { role = "system", content = systemPrefix },
 new { role = "user", content = userPrompt.ToString() }
 };

 var payload = new { model = model, messages = messages, stream = false };
 var payloadJson = JsonSerializer.Serialize(payload);

 using var req = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
 {
 Content = new StringContent(payloadJson, Encoding.UTF8, "application/json")
 };

 using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseContentRead);
 lastRaw = await resp.Content.ReadAsStringAsync();

 if (!resp.IsSuccessStatusCode)
 {
 _logger.LogWarning("OpenRouter returned status {Status} on attempt {Attempt}: {Content}", resp.StatusCode, attempt, lastRaw);
 await Task.Delay(300);
 continue;
 }

 // Try get candidate from OpenRouter response shape
 string? candidate = null;
 try
 {
 using var outer = JsonDocument.Parse(lastRaw);
 var root = outer.RootElement;

 if (root.TryGetProperty("choices", out var choices) && choices.ValueKind == JsonValueKind.Array && choices.GetArrayLength() >0)
 {
 var first = choices[0];
 if (first.TryGetProperty("message", out var message) && message.TryGetProperty("content", out var contentElem))
 {
 candidate = contentElem.GetString();
 }
 else if (first.TryGetProperty("text", out var textProp))
 {
 candidate = textProp.GetString();
 }
 }
 }
 catch (JsonException)
 {
 candidate = lastRaw.Trim();
 }

 if (string.IsNullOrWhiteSpace(candidate)) candidate = lastRaw.Trim();

 //1) try parse directly
 if (TryParseProgramAiResult(candidate, request, out var parsedDirect)) return parsedDirect;

 //2) try extract JSON substring and parse
 if (TryExtractJson(candidate, out var extracted) && TryParseProgramAiResult(extracted, request, out var parsedExtracted)) return parsedExtracted;

 //3) ask model to correct (prefix instruction) and retry
 if (attempt < maxAttempts)
 {
 userPrompt.Insert(0, "The previous response was not valid JSON. PLEASE RETURN ONLY VALID JSON matching the required schema exactly and nothing else.\n");
 await Task.Delay(500);
 continue;
 }
 }

 // fallback skeleton
 var fallback = new ProgramAiResult { IsValidJson = false };
 fallback.Program = new ProgramCreateRequest
 {
 TrainerProfileId = request.TrainerProfileId,
 Title = $"AI Program - {request.Goal}",
 Description = request.Notes ?? string.Empty,
 DurationWeeks = request.DurationWeeks,
 Type = ITI.Gymunity.FP.Domain.Models.Enums.ProgramType.Workout,
 IsPublic = true
 };

 for (int w =1; w <= request.DurationWeeks; w++)
 {
 var week = new ProgramWeekDto { WeekNumber = w };
 for (int d =1; d <= request.DaysPerWeek; d++)
 {
 var day = new ProgramDayDto { DayNumber = d, Title = $"Day {d}" };
 for (int e =0; e < request.ExercisesPerDay; e++)
 {
 day.Exercises.Add(new ProgramDayExerciseDto { Name = "Exercise", Sets = "3", Reps = "8-12" });
 }
 week.Days.Add(day);
 }
 fallback.Weeks.Add(week);
 }

 return fallback;
 }

 private static bool TryParseProgramAiResult(string candidate, ProgramAiCreateRequest request, out ProgramAiResult result)
 {
 result = new ProgramAiResult { IsValidJson = false };
 if (string.IsNullOrWhiteSpace(candidate)) return false;

 try
 {
 using var doc = JsonDocument.Parse(candidate);
 var root = doc.RootElement;

 var r = new ProgramAiResult { IsValidJson = true };
 r.Program = new ProgramCreateRequest
 {
 TrainerProfileId = request.TrainerProfileId,
 Title = root.TryGetProperty("programName", out var pn) && pn.ValueKind == JsonValueKind.String ? pn.GetString() ?? $"AI Program - {request.Goal}" : (root.TryGetProperty("title", out var t) && t.ValueKind == JsonValueKind.String ? t.GetString() ?? $"AI Program - {request.Goal}" : $"AI Program - {request.Goal}"),
 Description = root.TryGetProperty("notes", out var notes) && notes.ValueKind == JsonValueKind.String ? notes.GetString() ?? string.Empty : (root.TryGetProperty("description", out var dsc) && dsc.ValueKind == JsonValueKind.String ? dsc.GetString() ?? string.Empty : string.Empty),
 DurationWeeks = root.TryGetProperty("durationWeeks", out var dw) && dw.ValueKind == JsonValueKind.Number ? dw.GetInt32() : request.DurationWeeks,
 Type = ITI.Gymunity.FP.Domain.Models.Enums.ProgramType.Workout,
 IsPublic = true
 };

 // parse schedule
 if (root.TryGetProperty("schedule", out var schedule) && schedule.ValueKind == JsonValueKind.Array)
 {
 foreach (var w in schedule.EnumerateArray())
 {
 var weekDto = new ProgramWeekDto();
 if (w.TryGetProperty("week", out var wn) && wn.ValueKind == JsonValueKind.Number) weekDto.WeekNumber = wn.GetInt32();

 if (w.TryGetProperty("days", out var days) && days.ValueKind == JsonValueKind.Array)
 {
 foreach (var d in days.EnumerateArray())
 {
 var dayDto = new ProgramDayDto();
 if (d.TryGetProperty("dayNumber", out var dn) && dn.ValueKind == JsonValueKind.Number) dayDto.DayNumber = dn.GetInt32();

 if (d.TryGetProperty("exercises", out var exs) && exs.ValueKind == JsonValueKind.Array)
 {
 foreach (var ex in exs.EnumerateArray())
 {
 var exDto = new ProgramDayExerciseDto();
 if (ex.TryGetProperty("name", out var nm) && nm.ValueKind == JsonValueKind.String) exDto.Name = nm.GetString() ?? string.Empty;
 if (ex.TryGetProperty("sets", out var sets) && sets.ValueKind == JsonValueKind.Number) exDto.Sets = sets.GetInt32().ToString();
 else if (ex.TryGetProperty("sets", out var setsStr) && setsStr.ValueKind == JsonValueKind.String) exDto.Sets = setsStr.GetString();

 if (ex.TryGetProperty("reps", out var reps) && reps.ValueKind == JsonValueKind.Number) exDto.Reps = reps.GetInt32().ToString();
 else if (ex.TryGetProperty("reps", out var repsStr) && repsStr.ValueKind == JsonValueKind.String) exDto.Reps = repsStr.GetString();

 dayDto.Exercises.Add(exDto);
 }
 }

 weekDto.Days.Add(dayDto);
 }
 }

 r.Weeks.Add(weekDto);
 }
 }
 else if (root.TryGetProperty("weeks", out var weeksElem) && weeksElem.ValueKind == JsonValueKind.Array)
 {
 foreach (var w in weeksElem.EnumerateArray())
 {
 var weekDto = new ProgramWeekDto();
 if (w.TryGetProperty("weekNumber", out var wn) && wn.ValueKind == JsonValueKind.Number) weekDto.WeekNumber = wn.GetInt32();

 if (w.TryGetProperty("days", out var daysElem) && daysElem.ValueKind == JsonValueKind.Array)
 {
 foreach (var d in daysElem.EnumerateArray())
 {
 var dayDto = new ProgramDayDto();
 if (d.TryGetProperty("dayNumber", out var dn) && dn.ValueKind == JsonValueKind.Number) dayDto.DayNumber = dn.GetInt32();
 if (d.TryGetProperty("title", out var dt) && dt.ValueKind == JsonValueKind.String) dayDto.Title = dt.GetString();

 if (d.TryGetProperty("exercises", out var exElem) && exElem.ValueKind == JsonValueKind.Array)
 {
 foreach (var ex in exElem.EnumerateArray())
 {
 var exDto = new ProgramDayExerciseDto
 {
 Name = ex.TryGetProperty("name", out var nm) && nm.ValueKind == JsonValueKind.String ? nm.GetString() ?? string.Empty : string.Empty,
 Sets = ex.TryGetProperty("sets", out var sets2) ? sets2.GetString() : null,
 Reps = ex.TryGetProperty("reps", out var reps2) ? reps2.GetString() : null,
 Notes = ex.TryGetProperty("notes", out var notesProp) ? notesProp.GetString() : null
 };
 dayDto.Exercises.Add(exDto);
 }
 }

 weekDto.Days.Add(dayDto);
 }
 }

 r.Weeks.Add(weekDto);
 }
 }

 result = r;
 return true;
 }
 catch (JsonException)
 {
 result = new ProgramAiResult { IsValidJson = false };
 return false;
 }
 catch (Exception)
 {
 result = new ProgramAiResult { IsValidJson = false };
 return false;
 }
 }

 // Extract first balanced JSON object or array from input
 private static bool TryExtractJson(string input, out string json)
 {
 json = string.Empty;
 if (string.IsNullOrWhiteSpace(input)) return false;

 int startObj = input.IndexOf('{');
 int startArr = input.IndexOf('[');
 if (startObj == -1 && startArr == -1) return false;

 int chosenStart;
 char openChar, closeChar;
 if (startObj == -1 || (startArr != -1 && startArr < startObj))
 {
 chosenStart = startArr;
 openChar = '['; closeChar = ']';
 }
 else
 {
 chosenStart = startObj;
 openChar = '{'; closeChar = '}';
 }

 int depth =0;
 for (int i = chosenStart; i < input.Length; i++)
 {
 if (input[i] == openChar) depth++;
 else if (input[i] == closeChar) depth--;

 if (depth ==0)
 {
 var candidate = input.Substring(chosenStart, i - chosenStart +1).Trim();
 try
 {
 using var doc = JsonDocument.Parse(candidate);
 json = candidate;
 return true;
 }
 catch (JsonException)
 {
 // continue searching
 }
 }
 }

 return false;
 }
 }
}
