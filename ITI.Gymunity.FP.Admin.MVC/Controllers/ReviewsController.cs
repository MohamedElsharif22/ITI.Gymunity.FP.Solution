using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI.Gymunity.FP.Admin.MVC.ViewModels.Reviews;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.Specefications.Admin;

namespace ITI.Gymunity.FP.Admin.MVC.Controllers
{
    /// <summary>
    /// Admin Reviews Controller
    /// Manages trainer review approval and rejection
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class ReviewsController : BaseAdminController
    {
        private readonly ILogger<ReviewsController> _logger;
        private readonly IReviewAdminService _reviewService;

        public ReviewsController(
            ILogger<ReviewsController> logger,
            IReviewAdminService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        /// <summary>
        /// Displays pending reviews awaiting approval
        /// </summary>
        [HttpGet("reviews/pending")]
        public async Task<IActionResult> Pending(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Pending Reviews");
                SetBreadcrumbs("Dashboard", "Reviews", "Pending");

                var specs = new PendingReviewsSpecs(pageNumber: pageNumber, pageSize: pageSize);
                var reviews = await _reviewService.GetAllPendingAsync();

                var model = new ReviewsListViewModel
                {
                    Reviews = reviews.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = reviews.Count(),
                    FilterType = "Pending"
                };

                _logger.LogInformation("Pending reviews list accessed by user: {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading pending reviews");
                ShowErrorMessage("An error occurred while loading pending reviews");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays all reviews (approved and pending)
        /// </summary>
        [HttpGet("reviews")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Manage Reviews");
                SetBreadcrumbs("Dashboard", "Reviews");

                var pendingReviews = await _reviewService.GetAllPendingAsync();
                
                // Get count of all pending reviews (not just on this page)
                var allPendingReviews = await _reviewService.GetAllPendingAsync();
                var totalPendingCount = allPendingReviews.Count;

                // Get count of unique trainers with pending reviews
                var totalUniqueUsers = allPendingReviews.Select(r => r.TrainerId).Distinct().Count();

                var model = new ReviewsListViewModel
                {
                    Reviews = pendingReviews.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = pendingReviews.Count(),
                    PendingCount = totalPendingCount,
                    TotalUniqueUsers = totalUniqueUsers,
                    FilterType = "Pending"
                };

                _logger.LogInformation("Reviews list accessed by user: {User}", User.Identity?.Name);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reviews list");
                ShowErrorMessage("An error occurred while loading reviews");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Approves a pending review
        /// </summary>
        [HttpPost("reviews/{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var result = await _reviewService.ApproveAsync(id);
                if (result == null)
                    return BadRequest(new { success = false, message = "Failed to approve review" });

                ShowSuccessMessage("Review approved successfully");
                _logger.LogInformation("Review {ReviewId} approved by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Review approved successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving review {ReviewId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Rejects a pending review
        /// </summary>
        [HttpPost("reviews/{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var result = await _reviewService.RejectAsync(id);
                if (result == null)
                    return BadRequest(new { success = false, message = "Failed to reject review" });

                ShowSuccessMessage("Review rejected successfully");
                _logger.LogInformation("Review {ReviewId} rejected by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Review rejected successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting review {ReviewId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a review permanently
        /// </summary>
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _reviewService.DeletePermanentAsync(id);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to delete review" });

                ShowSuccessMessage("Review deleted successfully");
                _logger.LogInformation("Review {ReviewId} deleted by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Review deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {ReviewId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AJAX endpoint for getting pending reviews as JSON
        /// </summary>
        [HttpGet("reviews/pending-json")]
        public async Task<IActionResult> GetPendingReviewsJson(
            int draw = 1,
            int start = 0,
            int length = 10)
        {
            try
            {
                var pageNumber = (start / length) + 1;
                var reviews = await _reviewService.GetAllPendingAsync();

                var paginatedReviews = reviews
                    .Skip((pageNumber - 1) * length)
                    .Take(length)
                    .ToList();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = reviews.Count,
                    recordsFiltered = reviews.Count,
                    data = paginatedReviews
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending reviews JSON");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
