using ITI.Gymunity.FP.Application.DTOs.Trainer;

namespace ITI.Gymunity.FP.Admin.MVC.ViewModels.Reviews
{
    public class ReviewsListViewModel
    {
        public List<TrainerReviewResponse> Reviews { get; set; } = new List<TrainerReviewResponse>();
        
        public int PageNumber { get; set; } = 1;
        
        public int PageSize { get; set; } = 10;
        
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// Total count of pending reviews across all pages
        /// </summary>
        public int PendingCount { get; set; } = 0;

        /// <summary>
        /// Total count of unique trainers/users with reviews
        /// </summary>
        public int TotalUniqueUsers { get; set; } = 0;

        /// <summary>
        /// Filter type applied (e.g., "Pending", "All")
        /// </summary>
        public string FilterType { get; set; } = "Pending";

        /// <summary>
        /// Number of reviews shown on current page
        /// </summary>
        public int CurrentPageReviewCount => Reviews.Count;

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

        /// <summary>
        /// Check if there are more pages
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Check if there are previous pages
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Get average rating from displayed reviews
        /// </summary>
        public double AverageRating => Reviews.Count > 0 
            ? Reviews.Average(r => r.Rating) 
            : 0;

        /// <summary>
        /// Get the start item number for current page
        /// </summary>
        public int StartItemNumber => (PageNumber - 1) * PageSize + 1;

        /// <summary>
        /// Get the end item number for current page
        /// </summary>
        public int EndItemNumber => Math.Min(StartItemNumber + CurrentPageReviewCount - 1, TotalCount);

        /// <summary>
        /// Get the count of unique trainers on the current page
        /// </summary>
        public int CurrentPageUniqueTrainerCount => Reviews
            .GroupBy(r => r.TrainerId)
            .Select(g => g.First())
            .Count();
    }
}
