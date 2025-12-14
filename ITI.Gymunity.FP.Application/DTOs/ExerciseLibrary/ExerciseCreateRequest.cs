using System;

namespace ITI.Gymunity.FP.Application.DTOs.ExerciseLibrary
{
 public class ExerciseCreateRequest
 {
 public string Name { get; set; } = null!;
 public string Category { get; set; } = null!;
 public string MuscleGroup { get; set; } = null!;
 public string? Equipment { get; set; }
 public string? VideoDemoUrl { get; set; }
 public string? ThumbnailUrl { get; set; }
 public bool IsCustom { get; set; } = true;
 public string? TrainerId { get; set; }
 }
}
