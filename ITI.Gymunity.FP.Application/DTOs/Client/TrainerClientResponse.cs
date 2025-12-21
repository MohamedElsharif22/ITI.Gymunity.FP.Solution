using System;

namespace ITI.Gymunity.FP.Application.DTOs.Client
{
 public class TrainerClientResponse
 {
 public int Id { get; set; }
 public string UserId { get; set; } = null!;
 public string UserName { get; set; } = null!;
 public string Handle { get; set; } = null!;
 public string Bio { get; set; } = string.Empty;
 public string? CoverImageUrl { get; set; }
 public decimal RatingAverage { get; set; }
 public int TotalClients { get; set; }
 public int YearsExperience { get; set; }
 }
}
