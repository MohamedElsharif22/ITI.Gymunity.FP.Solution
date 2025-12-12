

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Program Day Response
    public class ProgramDayResponse
    {
        public int Id { get; set; }
        public int ProgramWeekId { get; set; }
        public int DayNumber { get; set; }
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public int TotalExercises { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }


}