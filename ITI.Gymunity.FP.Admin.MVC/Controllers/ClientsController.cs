using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI.Gymunity.FP.Admin.MVC.ViewModels.Clients;
using ITI.Gymunity.FP.Application.Services.Admin;
using ITI.Gymunity.FP.Application.Specefications.Admin;

namespace ITI.Gymunity.FP.Admin.MVC.Controllers
{
    /// <summary>
    /// Admin Clients Controller
    /// Manages client accounts, suspension, and lifecycle
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class ClientsController : BaseAdminController
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly ClientAdminService _clientService;

        public ClientsController(
            ILogger<ClientsController> logger,
            ClientAdminService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        /// <summary>
        /// Displays list of all clients with filtering and pagination
        /// Returns full view for initial load, partial for AJAX requests
        /// </summary>
        [HttpGet("clients")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            bool? isActive = null,
            string? searchTerm = null)
        {
            try
            {
                SetPageTitle("Manage Clients");
                SetBreadcrumbs("Dashboard", "Clients");

                var specs = new ClientFilterSpecs(
                    isActive: isActive,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerm: searchTerm);

                var clients = await _clientService.GetAllClientsAsync(specs);
                var totalCount = await _clientService.GetClientCountAsync(specs);

                var model = new ClientsListViewModel
                {
                    Clients = clients.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    SearchTerm = searchTerm,
                    IsActiveFilter = isActive
                };

                _logger.LogInformation("Clients list accessed by user: {User}", User.Identity?.Name);

                // Check if this is an AJAX request
                if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                {
                    // Return only the partial view (table + pagination) for AJAX
                    return PartialView("_ClientsTablePartial", model);
                }

                // Return full view for initial page load
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading clients list");
                ShowErrorMessage("An error occurred while loading clients");

                // If AJAX request, return error response
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }

                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays detailed view of a specific client
        /// </summary>
        [HttpGet("clients/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                SetPageTitle("Client Details");
                SetBreadcrumbs("Dashboard", "Clients", "Details");

                var client = await _clientService.GetClientByIdAsync(id);
                if (client == null)
                {
                    ShowErrorMessage("Client not found");
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogInformation("Client {ClientId} details viewed by {User}", id, User.Identity?.Name);
                return View(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading client details for ID {ClientId}", id);
                ShowErrorMessage("An error occurred while loading client details");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Suspends a client account
        /// </summary>
        [HttpPost("clients/{id}/suspend")]
        public async Task<IActionResult> Suspend(string id)
        {
            try
            {
                if (!int.TryParse(id, out var clientId))
                    return BadRequest(new { success = false, message = "Invalid client ID" });

                var result = await _clientService.SuspendClientAsync(clientId);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to suspend client" });

                ShowSuccessMessage("Client suspended successfully");
                _logger.LogInformation("Client {ClientId} suspended by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Client suspended successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending client {ClientId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Reactivates a suspended client account
        /// </summary>
        [HttpPost("clients/{id}/reactivate")]
        public async Task<IActionResult> Reactivate(string id)
        {
            try
            {
                if (!int.TryParse(id, out var clientId))
                    return BadRequest(new { success = false, message = "Invalid client ID" });

                var result = await _clientService.ReactivateClientAsync(clientId);
                if (!result)
                    return BadRequest(new { success = false, message = "Failed to reactivate client" });

                ShowSuccessMessage("Client reactivated successfully");
                _logger.LogInformation("Client {ClientId} reactivated by {User}", id, User.Identity?.Name);
                return Ok(new { success = true, message = "Client reactivated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reactivating client {ClientId}", id);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Displays active clients
        /// </summary>
        [HttpGet("clients/active")]
        public async Task<IActionResult> Active(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Active Clients");
                SetBreadcrumbs("Dashboard", "Clients", "Active");

                var clients = await _clientService.GetActiveClientsAsync(pageNumber, pageSize);
                var totalCount = await _clientService.GetClientCountAsync(
                    new ClientFilterSpecs(isActive: true));

                var model = new ClientsListViewModel
                {
                    Clients = clients.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    IsActiveFilter = true
                };

                _logger.LogInformation("Active clients list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading active clients");
                ShowErrorMessage("An error occurred while loading active clients");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Displays suspended clients
        /// </summary>
        [HttpGet("clients/suspended")]
        public async Task<IActionResult> Suspended(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetPageTitle("Suspended Clients");
                SetBreadcrumbs("Dashboard", "Clients", "Suspended");

                var clients = await _clientService.GetInactiveClientsAsync(pageNumber, pageSize);
                var totalCount = await _clientService.GetClientCountAsync(
                    new ClientFilterSpecs(isActive: false));

                var model = new ClientsListViewModel
                {
                    Clients = clients.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    IsActiveFilter = false
                };

                _logger.LogInformation("Suspended clients list accessed by {User}", User.Identity?.Name);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading suspended clients");
                ShowErrorMessage("An error occurred while loading suspended clients");
                return RedirectToDashboard();
            }
        }

        /// <summary>
        /// Searches clients by name, email, or username
        /// </summary>
        [HttpGet("clients/search")]
        public async Task<IActionResult> Search(
            string? q,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                    return RedirectToAction(nameof(Index));

                var clients = await _clientService.SearchClientsAsync(q, pageNumber, pageSize);

                var model = new ClientsListViewModel
                {
                    Clients = clients.ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = q
                };

                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching clients with term {SearchTerm}", q);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AJAX endpoint for DataTable - Returns clients as JSON
        /// </summary>
        [HttpGet("clients/list-json")]
        public async Task<IActionResult> GetClientsJson(
            int draw = 1,
            int start = 0,
            int length = 10,
            string? search = null)
        {
            try
            {
                var pageNumber = (start / length) + 1;
                var specs = new ClientFilterSpecs(
                    pageNumber: pageNumber,
                    pageSize: length,
                    searchTerm: search);

                var clients = await _clientService.GetAllClientsAsync(specs);
                var totalCount = await _clientService.GetTotalClientCountAsync();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = clients
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting clients JSON");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
