using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITI.Gymunity.FP.Admin.MVC.ViewModels.Trainers;
using ITI.Gymunity.FP.Application.Services.Admin;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Application.DTOs.Trainer;

namespace ITI.Gymunity.FP.Admin.MVC.Controllers
{
    /// <summary>
    /// Admin Trainers Controller
    /// Manages trainer profiles, verification, and account actions
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class TrainersController : BaseAdminController
    {
        private readonly ILogger<TrainersController> _logger;
        private readonly TrainerAdminService _trainerService;

        public TrainersController(
            ILogger<TrainersController> logger,
            TrainerAdminService trainerService)
        {
            _logger = logger;
            _trainerService = trainerService;
        }

        /// <summary>
        /// Displays list of all trainers with filtering and pagination
        /// </summary>
        [HttpGet("trainers")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            bool? isVerified = null,
            bool? isSuspended = null,
            string? searchTerm = null)
        {
            try
            {
                SetPageTitle("Manage Trainers");
                SetBreadcrumbs("Dashboard", "Trainers");

                var specs = new TrainerFilterSpecs(
                    isVerified: isVerified,
                    isSuspended: isSuspended,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerm: searchTerm);

                var trainers = await _trainerService.GetAllTrainersAsync(specs);
                var totalCount = await _trainerService.GetTrainerCountAsync(specs);

                var model = new TrainersListViewModel
                {
                    Trainers = trainers.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    SearchTerm = searchTerm,
                    IsVerifiedFilter = isVerified,
                    IsSuspendedFilter = isSuspended
                };

                _logger.LogInformation("Trainers list accessed by user: {User}", User.Identity?.Name);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading trainers list");
                ShowErrorMessage("An error occurred while loading trainers");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// AJAX endpoint for filtering trainers without page reload
        /// </summary>
        [HttpGet("trainers/filter")]
        public async Task<IActionResult> FilterTrainers(
            int pageNumber = 1,
            int pageSize = 10,
            bool? isVerified = null,
            bool? isSuspended = null,
            string? searchTerm = null)
        {
            try
            {
                var specs = new TrainerFilterSpecs(
                    isVerified: isVerified,
                    isSuspended: isSuspended,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerm: searchTerm);

                var trainers = await _trainerService.GetAllTrainersAsync(specs);
                var totalCount = await _trainerService.GetTrainerCountAsync(specs);

                var model = new TrainersListViewModel
                {
                    Trainers = trainers.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    SearchTerm = searchTerm,
                    IsVerifiedFilter = isVerified,
                    IsSuspendedFilter = isSuspended
                };

                return Json(new
                {
                    success = true,
                    data = model,
                    html = await RenderPartialViewToStringAsync("_TrainersTablePartial", model)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering trainers");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Displays detailed view of a specific trainer
        /// </summary>
        [HttpGet("trainers/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                SetPageTitle("Trainer Details");
                SetBreadcrumbs("Dashboard", "Trainers", "Details");

                var trainer = await _trainerService.GetTrainerByIdAsync(id);
                if (trainer == null)
                {
                    ShowErrorMessage("Trainer not found");
                    return RedirectToAction(nameof(Index));
                }

                var vm = new TrainerDetailsViewModel
                {
                    Id = trainer.Id,
                    UserId = trainer.UserId,
                    UserName = trainer.UserName,
                    Handle = trainer.Handle,
                    Bio = trainer.Bio,
                    CoverImageUrl = trainer.CoverImageUrl,
                    VideoIntroUrl = trainer.VideoIntroUrl,
                    BrandingColors = trainer.BrandingColors,
                    IsVerified = trainer.IsVerified,
                    VerifiedAt = trainer.VerifiedAt,
                    IsSuspended = trainer.IsSuspended,
                    SuspendedAt = trainer.SuspendedAt,
                    RatingAverage = trainer.RatingAverage,
                    TotalClients = trainer.TotalClients,
                    YearsExperience = trainer.YearsExperience,
                    StatusImageUrl = trainer.StatusImageUrl,
                    StatusDescription = trainer.StatusDescription,
                    AvailableBalance = trainer.AvailableBalance,
                    CreatedAt = trainer.CreatedAt
                };

                _logger.LogInformation("Trainer {TrainerId} details viewed by {User}", id, User.Identity?.Name);
                return View("~/Views/Trainers/Details.cshtml", vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading trainer details for ID {TrainerId}", id);
                ShowErrorMessage("An error occurred while loading trainer details");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Verifies a trainer profile (marks as verified)
        /// </summary>
        [HttpPost("trainers/{id}/verify")]
        public async Task<IActionResult> Verify(int id)
        {
            try
            {
                var result = await _trainerService.VerifyTrainerAsync(id);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to verify trainer" });

                ShowSuccessMessage("Trainer verified successfully");
                _logger.LogInformation("Trainer {TrainerId} verified by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Trainer verified successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying trainer {TrainerId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Rejects a trainer profile
        /// </summary>
        [HttpPost("trainers/{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var result = await _trainerService.RejectTrainerAsync(id);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to reject trainer" });

                ShowSuccessMessage("Trainer rejected successfully");
                _logger.LogInformation("Trainer {TrainerId} rejected by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Trainer rejected successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting trainer {TrainerId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Suspends a trainer account
        /// </summary>
        [HttpPost("trainers/{id}/suspend")]
        public async Task<IActionResult> Suspend(int id)
        {
            try
            {
                var result = await _trainerService.SuspendTrainerAsync(id, suspend: true);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to suspend trainer" });

                ShowSuccessMessage("Trainer suspended successfully");
                _logger.LogInformation("Trainer {TrainerId} suspended by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Trainer suspended successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending trainer {TrainerId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Reactivates a suspended trainer account
        /// </summary>
        [HttpPost("trainers/{id}/reactivate")]
        public async Task<IActionResult> Reactivate(int id)
        {
            try
            {
                var result = await _trainerService.SuspendTrainerAsync(id, suspend: false);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to reactivate trainer" });

                ShowSuccessMessage("Trainer reactivated successfully");
                _logger.LogInformation("Trainer {TrainerId} reactivated by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Trainer reactivated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reactivating trainer {TrainerId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Displays pending trainers awaiting verification
        /// </summary>
        [HttpGet("trainers/pending")]
        public async Task<IActionResult> Pending(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Pending Trainers");
                SetBreadcrumbs("Dashboard", "Trainers", "Pending");

                var trainers = await _trainerService.GetPendingTrainersAsync(pageNumber, pageSize);
                var totalCount = await _trainerService.GetPendingTrainerCountAsync();

                var model = new TrainersListViewModel
                {
                    Trainers = trainers.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    IsVerifiedFilter = false
                };

                _logger.LogInformation("Pending trainers list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading pending trainers");
                ShowErrorMessage("An error occurred while loading pending trainers");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Searches trainers by name, email, or handle
        /// </summary>
        [HttpGet("trainers/search")]
        public async Task<IActionResult> Search(
            string? q,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                    return RedirectToAction(nameof(Index));

                var trainers = await _trainerService.SearchTrainersAsync(q, pageNumber, pageSize);

                var model = new TrainersListViewModel
                {
                    Trainers = trainers.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = q
                };

                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching trainers with term {SearchTerm}", q);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AJAX endpoint for DataTable - Returns trainers as JSON
        /// </summary>
        [HttpGet("trainers/list-json")]
        public async Task<IActionResult> GetTrainersJson(
            int draw = 1,
            int start = 0,
            int length = 10,
            string? search = null)
        {
            try
            {
                var pageNumber = (start / length) + 1;
                var specs = new TrainerFilterSpecs(
                    pageNumber: pageNumber,
                    pageSize: length,
                    searchTerm: search);

                var trainers = await _trainerService.GetAllTrainersAsync(specs);
                var totalCount = await _trainerService.GetTrainerCountAsync(new TrainerFilterSpecs());

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = trainers
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trainers JSON");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Helper method to render partial view to string
        /// </summary>
        private async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(ControllerContext, viewName, false);
                
                if (!viewResult.Success)
                    throw new InvalidOperationException($"The view '{viewName}' was not found.");

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
