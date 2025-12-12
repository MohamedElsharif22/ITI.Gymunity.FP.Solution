using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Update Program Request
    public class UpdateProgramRequest
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public ProgramType? Type { get; set; }

        [Range(1, 52)]
        public int? DurationWeeks { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        public bool? IsPublic { get; set; }

        public bool? IsActive { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxClients { get; set; }

        public IFormFile? Image { get; set; }

        public bool RemoveImage { get; set; } = false;
    }


}
