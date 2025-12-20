using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Infrastructure.DTOs.Trainer
{
    public class UpdateStatusRequest
    {
        public IFormFile? StatusImage { get; set; }

        [StringLength(200)]
        public string? StatusDescription { get; set; }
    }
}