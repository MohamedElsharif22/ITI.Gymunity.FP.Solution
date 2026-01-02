using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.User;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.Admin
{
    /// <summary>
    /// Admin service for managing client profiles and accounts
    /// </summary>
    public class ClientAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientAdminService> _logger;

        public ClientAdminService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ClientAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all clients with optional filtering and pagination
        /// </summary>
        public async Task<IEnumerable<ClientResponse>> GetAllClientsAsync(
            ClientFilterSpecs specs)
        {
            try
            {
                var clients = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<ClientResponse>>(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients with specs");
                throw;
            }
        }

        /// <summary>
        /// Get client by ID
        /// </summary>
        public async Task<ClientResponse?> GetClientByIdAsync(int clientId)
        {
            try
            {
                var client = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetByIdAsync(clientId);

                if (client == null)
                    return null;

                return _mapper.Map<ClientResponse>(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving client with ID {ClientId}", clientId);
                throw;
            }
        }

        /// <summary>
        /// Get count of clients matching specification
        /// </summary>
        public async Task<int> GetClientCountAsync(ClientFilterSpecs specs)
        {
            try
            {
                return await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client count");
                throw;
            }
        }

        /// <summary>
        /// Suspend a client account
        /// </summary>
        public async Task<bool> SuspendClientAsync(int clientId)
        {
            try
            {
                var client = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetByIdAsync(clientId);

                if (client == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for suspension", clientId);
                    return false;
                }

                client.IsDeleted = true;

                _unitOfWork.Repository<ClientProfile>().Update(client);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Client {ClientId} suspended successfully", clientId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending client {ClientId}", clientId);
                throw;
            }
        }

        /// <summary>
        /// Reactivate a suspended client account
        /// </summary>
        public async Task<bool> ReactivateClientAsync(int clientId)
        {
            try
            {
                var client = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetByIdAsync(clientId);

                if (client == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for reactivation", clientId);
                    return false;
                }

                client.IsDeleted = false;

                _unitOfWork.Repository<ClientProfile>().Update(client);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Client {ClientId} reactivated successfully", clientId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reactivating client {ClientId}", clientId);
                throw;
            }
        }

        /// <summary>
        /// Get active clients
        /// </summary>
        public async Task<IEnumerable<ClientResponse>> GetActiveClientsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new ClientFilterSpecs(
                    isActive: true,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var clients = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<ClientResponse>>(clients.Select(c => c.User));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active clients");
                throw;
            }
        }

        /// <summary>
        /// Get inactive/suspended clients
        /// </summary>
        public async Task<IEnumerable<ClientResponse>> GetInactiveClientsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new ClientFilterSpecs(
                    isActive: false,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var clients = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<ClientResponse>>(clients.Select(c => c.User));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inactive clients");
                throw;
            }
        }

        /// <summary>
        /// Search clients by name, email, or username
        /// </summary>
        public async Task<IEnumerable<ClientResponse>> SearchClientsAsync(
            string searchTerm,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new ClientFilterSpecs(
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerm: searchTerm);

                var clients = await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<ClientResponse>>(clients.Select(c => c.User));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching clients with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        /// <summary>
        /// Get total client count
        /// </summary>
        public async Task<int> GetTotalClientCountAsync()
        {
            try
            {
                var specs = new ClientFilterSpecs();
                return await _unitOfWork
                    .Repository<ClientProfile>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total client count");
                throw;
            }
        }
    }
}
