// ============================================
// DTOs for Program
// Location: Application/DTOs/Program/
// ============================================
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Create Program Request
    public class CreateProgramRequest
    {
        [Required]
        public string TrainerId { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public ProgramType Type { get; set; }

        [Range(1, 52)]
        public int DurationWeeks { get; set; } = 1;

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        public bool IsPublic { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int? MaxClients { get; set; }

        public IFormFile? Image { get; set; }
    }

}