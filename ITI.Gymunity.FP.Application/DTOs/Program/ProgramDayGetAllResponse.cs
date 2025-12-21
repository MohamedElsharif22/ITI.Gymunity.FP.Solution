using System;
using System.Collections.Generic;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
 public class ProgramDayGetAllResponse
 {
 public int Id { get; set; }
 public int ProgramWeekId { get; set; }
 public int DayNumber { get; set; }
 public string? Title { get; set; }
 public string? Notes { get; set; }
 public ICollection<ProgramDayExerciseGetAllResponse> Exercises { get; set; } = new List<ProgramDayExerciseGetAllResponse>();
 }
}
