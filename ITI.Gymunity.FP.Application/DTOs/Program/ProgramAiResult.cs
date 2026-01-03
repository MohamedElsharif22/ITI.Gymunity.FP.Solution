using System.Collections.Generic;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
 public class ProgramAiResult
 {
 public ProgramCreateRequest Program { get; set; } = new ProgramCreateRequest();
 public List<ProgramWeekDto> Weeks { get; set; } = new List<ProgramWeekDto>();
 // Indicates whether the AI response was valid JSON and parsed successfully
 public bool IsValidJson { get; set; } = false;
 }

 public class ProgramWeekDto
 {
 public int WeekNumber { get; set; }
 public List<ProgramDayDto> Days { get; set; } = new List<ProgramDayDto>();
 }

 public class ProgramDayDto
 {
 public int DayNumber { get; set; }
 public string? Title { get; set; }
 public List<ProgramDayExerciseDto> Exercises { get; set; } = new List<ProgramDayExerciseDto>();
 }

 public class ProgramDayExerciseDto
 {
 public string Name { get; set; } = null!;
 public string? Sets { get; set; }
 public string? Reps { get; set; }
 public string? Notes { get; set; }
 }
}
