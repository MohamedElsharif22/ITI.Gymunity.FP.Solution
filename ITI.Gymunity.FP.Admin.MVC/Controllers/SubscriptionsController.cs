using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI.Gymunity.FP.Admin.MVC.ViewModels.Subscriptions;
using ITI.Gymunity.FP.Application.Services.Admin;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.Admin.MVC.Controllers
{
    /// <summary>
    /// Admin Subscriptions Controller
    /// Manages subscription lifecycle and monitoring
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class SubscriptionsController : BaseAdminController
    {
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly SubscriptionAdminService _subscriptionService;

        public SubscriptionsController(
            ILogger<SubscriptionsController> logger,
            SubscriptionAdminService subscriptionService)
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Displays list of all subscriptions with filtering and pagination
        /// </summary>
        [HttpGet("subscriptions")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            SubscriptionStatus? status = null)
        {
            try
            {
                SetPageTitle("Manage Subscriptions");
                SetBreadcrumbs("Dashboard", "Subscriptions");

                var specs = new SubscriptionFilterSpecs(
                    status: status,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync(specs);
                var totalCount = await _subscriptionService.GetSubscriptionCountAsync(specs);

                var model = new SubscriptionsListViewModel
                {
                    Subscriptions = subscriptions.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    StatusFilter = status
                };

                _logger.LogInformation("Subscriptions list accessed by user: {User}", User.Identity?.Name);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading subscriptions list");
                ShowErrorMessage("An error occurred while loading subscriptions");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays detailed view of a specific subscription
        /// </summary>
        [HttpGet("subscriptions/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                SetPageTitle("Subscription Details");
                SetBreadcrumbs("Dashboard", "Subscriptions", "Details");

                var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
                if (subscription == null)
                {
                    ShowErrorMessage("Subscription not found");
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogInformation("Subscription {SubscriptionId} details viewed by {User}", id, User.Identity?.Name);
                return View(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading subscription details for ID {SubscriptionId}", id);
                ShowErrorMessage("An error occurred while loading subscription details");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays active subscriptions
        /// </summary>
        [HttpGet("subscriptions/active")]
        public async Task<IActionResult> Active(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Active Subscriptions");
                SetBreadcrumbs("Dashboard", "Subscriptions", "Active");

                var subscriptions = await _subscriptionService.GetActiveSubscriptionsAsync(pageNumber, pageSize);
                var totalCount = await _subscriptionService.GetActiveSubscriptionCountAsync();

                var model = new SubscriptionsListViewModel
                {
                    Subscriptions = subscriptions.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    StatusFilter = SubscriptionStatus.Active
                };

                _logger.LogInformation("Active subscriptions list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading active subscriptions");
                ShowErrorMessage("An error occurred while loading active subscriptions");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays inactive/canceled subscriptions
        /// </summary>
        [HttpGet("subscriptions/inactive")]
        public async Task<IActionResult> Inactive(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Inactive Subscriptions");
                SetBreadcrumbs("Dashboard", "Subscriptions", "Inactive");

                var subscriptions = await _subscriptionService.GetInactiveSubscriptionsAsync(pageNumber, pageSize);
                var totalCount = await _subscriptionService.GetSubscriptionCountAsync(
                    new SubscriptionFilterSpecs(status: SubscriptionStatus.Canceled));

                var model = new SubscriptionsListViewModel
                {
                    Subscriptions = subscriptions.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    StatusFilter = SubscriptionStatus.Canceled
                };

                _logger.LogInformation("Inactive subscriptions list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading inactive subscriptions");
                ShowErrorMessage("An error occurred while loading inactive subscriptions");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays unpaid subscriptions
        /// </summary>
        [HttpGet("subscriptions/unpaid")]
        public async Task<IActionResult> Unpaid(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Unpaid Subscriptions");
                SetBreadcrumbs("Dashboard", "Subscriptions", "Unpaid");

                var subscriptions = await _subscriptionService.GetUnpaidSubscriptionsAsync(pageNumber, pageSize);
                var totalCount = await _subscriptionService.GetSubscriptionCountAsync(
                    new SubscriptionFilterSpecs(status: SubscriptionStatus.Unpaid));

                var model = new SubscriptionsListViewModel
                {
                    Subscriptions = subscriptions.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    StatusFilter = SubscriptionStatus.Unpaid
                };

                _logger.LogInformation("Unpaid subscriptions list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading unpaid subscriptions");
                ShowErrorMessage("An error occurred while loading unpaid subscriptions");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Cancels a subscription (admin action)
        /// </summary>
        [HttpPost("subscriptions/{id}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] SubscriptionCancelRequest request)
        {
            try
            {
                var result = await _subscriptionService.CancelSubscriptionAsync(id, request?.Reason ?? "");
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to cancel subscription" });

                ShowSuccessMessage("Subscription canceled successfully");
                _logger.LogInformation("Subscription {SubscriptionId} canceled by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Subscription canceled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling subscription {SubscriptionId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Displays subscriptions expiring soon
        /// </summary>
        [HttpGet("subscriptions/expiring-soon")]
        public async Task<IActionResult> ExpiringSoon()
        {
            try
            {
                SetPageTitle("Subscriptions Expiring Soon");
                SetBreadcrumbs("Dashboard", "Subscriptions", "Expiring Soon");

                var subscriptions = await _subscriptionService.GetExpiringSoonSubscriptionsAsync();

                var model = new SubscriptionsListViewModel
                {
                    Subscriptions = subscriptions.ToList(),
                    PageNumber = 1,
                    PageSize = subscriptions.Count(),
                    TotalCount = subscriptions.Count()
                };

                _logger.LogInformation("Expiring soon subscriptions list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading expiring subscriptions");
                ShowErrorMessage("An error occurred while loading expiring subscriptions");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// AJAX endpoint for DataTable - Returns subscriptions as JSON
        /// </summary>
        [HttpGet("subscriptions/list-json")]
        public async Task<IActionResult> GetSubscriptionsJson(
            int draw = 1,
            int start = 0,
            int length = 10,
            string? status = null)
        {
            try
            {
                var pageNumber = (start / length) + 1;
                SubscriptionStatus? subscriptionStatus = null;

                if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<SubscriptionStatus>(status, out var parsedStatus))
                {
                    subscriptionStatus = parsedStatus;
                }

                var specs = new SubscriptionFilterSpecs(
                    status: subscriptionStatus,
                    pageNumber: pageNumber,
                    pageSize: length);

                var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync(specs);
                var totalCount = await _subscriptionService.GetSubscriptionCountAsync(new SubscriptionFilterSpecs());

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = subscriptions
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subscriptions JSON");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
