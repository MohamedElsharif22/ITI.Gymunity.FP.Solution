using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Update Exercise in Day Request
    public class UpdateExerciseInDayRequest
    {
        public int? OrderIndex { get; set; }
        public string? Sets { get; set; }
        public string? Reps { get; set; }
        public int? RestSeconds { get; set; }
        public string? Tempo { get; set; }
        public decimal? RPE { get; set; }
        public decimal? Percent1RM { get; set; }
        public string? Notes { get; set; }
        public string? VideoUrl { get; set; }
        public string? ExerciseDataJson { get; set; }
    }

}
