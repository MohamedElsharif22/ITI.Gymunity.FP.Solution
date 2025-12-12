// ============================================
// DTOs for ProgramWeek
// Location: Application/DTOs/Program/
// ============================================
using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Add Week Request
    public class AddWeekRequest
    {
        [Required]
        public int ProgramId { get; set; }
    }



}