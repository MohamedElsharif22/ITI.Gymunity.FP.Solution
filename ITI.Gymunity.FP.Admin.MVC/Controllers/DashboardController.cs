using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI.Gymunity.FP.Admin.MVC.ViewModels.Dashboard;
using ITI.Gymunity.FP.Admin.MVC.Services;

namespace ITI.Gymunity.FP.Admin.MVC.Controllers
{
    /// <summary>
    /// Admin Dashboard Controller
    /// Displays key metrics, charts, and system overview.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class DashboardController : BaseAdminController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DashboardStatisticsService _dashboardService;

        public DashboardController(
            ILogger<DashboardController> logger,
            DashboardStatisticsService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Displays the main dashboard with statistics and charts.
        /// </summary>
        [HttpGet("")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> Index()
        {
            try
            {
                SetPageTitle("Dashboard");
                SetBreadcrumbs();

                var model = await _dashboardService.GetDashboardOverviewAsync();

                _logger.LogInformation("Dashboard accessed by user: {User}", User.Identity?.Name);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");
                ShowErrorMessage("An error occurred while loading the dashboard");
                return View(new DashboardOverviewViewModel());
            }
        }

        /// <summary>
        /// Refreshes dashboard data (API endpoint).
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                var model = await _dashboardService.GetDashboardOverviewAsync();
                return Ok(new { success = true, message = "Dashboard refreshed successfully", data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing dashboard");
                return BadRequest(new { success = false, message = "Error refreshing dashboard" });
            }
        }

        /// <summary>
        /// Gets dashboard statistics (API endpoint).
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var model = await _dashboardService.GetDashboardOverviewAsync();
                return Ok(new
                {
                    totalUsers = model.TotalUsers,
                    totalTrainers = model.TotalTrainers,
                    activeSubscriptions = model.ActiveSubscriptions,
                    totalRevenue = model.TotalRevenue,
                    usersTrend = model.UsersTrend,
                    trainersTrend = model.TrainersTrend,
                    subscriptionsTrend = model.SubscriptionsTrend,
                    revenueTrend = model.RevenueTrend
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics");
                return BadRequest(new { success = false, message = "Error getting statistics" });
            }
        }

        /// <summary>
        /// Gets chart data for revenue trends.
        /// </summary>
        [HttpGet("chart-data")]
        public async Task<IActionResult> GetChartData([FromQuery] int days = 30)
        {
            try
            {
                if (days < 7 || days > 365)
                    days = 30;

                var chartData = await _dashboardService.GetRevenueChartDataAsync(days);
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chart data");
                return BadRequest(new { success = false, message = "Error getting chart data" });
            }
        }

        /// <summary>
        /// Gets user distribution chart data.
        /// </summary>
        [HttpGet("user-distribution")]
        public async Task<IActionResult> GetUserDistribution()
        {
            try
            {
                var chartData = await _dashboardService.GetUserDistributionChartDataAsync();
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user distribution data");
                return BadRequest(new { success = false, message = "Error getting user distribution data" });
            }
        }
    }
}
