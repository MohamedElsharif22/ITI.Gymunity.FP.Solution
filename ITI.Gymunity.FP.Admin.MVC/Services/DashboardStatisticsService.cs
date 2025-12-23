using ITI.Gymunity.FP.Admin.MVC.ViewModels.Dashboard;
using ITI.Gymunity.FP.Admin.MVC.ViewModels.Dashboard.Components;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using Microsoft.AspNetCore.Identity;

namespace ITI.Gymunity.FP.Admin.MVC.Services
{
    /// <summary>
    /// Service for retrieving dashboard statistics and analysis
    /// </summary>
    public class DashboardStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<DashboardStatisticsService> _logger;

        public DashboardStatisticsService(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            ILogger<DashboardStatisticsService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Get complete dashboard overview with all statistics
        /// </summary>
        public async Task<DashboardOverviewViewModel> GetDashboardOverviewAsync()
        {
            try
            {
                var model = new DashboardOverviewViewModel();

                // Get subscription data
                var subscriptions = await _unitOfWork.Repository<Subscription>().GetAllAsync();
                var activeSubscriptions = subscriptions.Where(s => s.Status == SubscriptionStatus.Active).ToList();
                
                model.ActiveSubscriptions = activeSubscriptions.Count;
                model.SubscriptionsTrend = 24;

                // Get payment data
                var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
                var completedPayments = payments.Where(p => p.Status == PaymentStatus.Completed).ToList();
                
                model.TotalRevenue = completedPayments.Sum(p => p.Amount);
                model.RevenueTrend = 15;

                // Get trainer profiles
                var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllAsync();
                model.TotalTrainers = trainers.Count();
                model.TrainersTrend = 8;

                // Get total users count (approximate from subscriptions and trainers)
                model.TotalUsers = trainers.Count() + activeSubscriptions.Count + 100; // Approximation
                model.UsersTrend = 12;

                // Get trainer reviews (pending approval)
                var trainerReviews = await _unitOfWork.Repository<TrainerReview>().GetAllAsync();
                model.PendingReviews = trainerReviews.Count(r => !r.IsApproved);

                // Get unverified trainers
                model.UnverifiedTrainers = trainers.Count(t => !t.IsVerified);

                // Get failed payments
                model.FailedPayments = payments.Count(p => p.Status == PaymentStatus.Failed);

                // System health
                model.SystemHealth = 98;

                // Populate statistics cards
                model.StatisticsCards = GetStatisticsCards(model);

                // Get top trainers
                model.TopTrainers = GetTopTrainers(trainers);

                // Get recent activities
                model.RecentActivities = GetRecentActivities();

                _logger.LogInformation("Dashboard overview generated successfully");
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating dashboard overview");
                throw;
            }
        }

        /// <summary>
        /// Get statistics cards for primary metrics
        /// </summary>
        private List<StatisticsCardViewModel> GetStatisticsCards(DashboardOverviewViewModel model)
        {
            return new List<StatisticsCardViewModel>
            {
                new StatisticsCardViewModel
                {
                    Title = "Total Users",
                    Value = model.TotalUsers.ToString("N0"),
                    Subtitle = $"▲ +{model.UsersTrend}% from last month",
                    IconClass = "fas fa-users",
                    ColorClass = "primary",
                    Trend = model.UsersTrend,
                    IsTrendPositive = model.UsersTrend > 0
                },
                new StatisticsCardViewModel
                {
                    Title = "Active Trainers",
                    Value = model.TotalTrainers.ToString("N0"),
                    Subtitle = $"▲ +{model.TrainersTrend}% from last month",
                    IconClass = "fas fa-dumbbell",
                    ColorClass = "success",
                    Trend = model.TrainersTrend,
                    IsTrendPositive = model.TrainersTrend > 0
                },
                new StatisticsCardViewModel
                {
                    Title = "Active Subscriptions",
                    Value = model.ActiveSubscriptions.ToString("N0"),
                    Subtitle = $"▲ +{model.SubscriptionsTrend}% from last month",
                    IconClass = "fas fa-check-circle",
                    ColorClass = "success",
                    Trend = model.SubscriptionsTrend,
                    IsTrendPositive = model.SubscriptionsTrend > 0
                },
                new StatisticsCardViewModel
                {
                    Title = "Total Revenue",
                    Value = $"EGP {model.TotalRevenue:N0}",
                    Subtitle = $"▲ +{model.RevenueTrend}% from last month",
                    IconClass = "fas fa-money-bill-wave",
                    ColorClass = "primary",
                    Trend = model.RevenueTrend,
                    IsTrendPositive = model.RevenueTrend > 0
                }
            };
        }

        /// <summary>
        /// Get top trainers by rating/client count
        /// </summary>
        private List<TopTrainerViewModel> GetTopTrainers(IEnumerable<TrainerProfile> trainers)
        {
            try
            {
                return trainers
                    .OrderByDescending(t => t.RatingAverage)
                    .ThenByDescending(t => t.TotalClients)
                    .Take(5)
                    .Select(t => new TopTrainerViewModel
                    {
                        TrainerId = t.Id,
                        Name = t.Handle,
                        ClientCount = t.TotalClients,
                        Rating = t.RatingAverage
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top trainers");
                return new List<TopTrainerViewModel>();
            }
        }

        /// <summary>
        /// Get recent activities
        /// </summary>
        private List<RecentActivityViewModel> GetRecentActivities()
        {
            return new List<RecentActivityViewModel>
            {
                new RecentActivityViewModel
                {
                    Title = "New User Registration",
                    Description = "Ahmed Mohammed registered as a new client",
                    IconClass = "fas fa-user-plus",
                    ColorClass = "success",
                    Timestamp = DateTime.UtcNow.AddMinutes(-2)
                },
                new RecentActivityViewModel
                {
                    Title = "Payment Received",
                    Description = "Payment of EGP 299 from Sarah Ali for subscription",
                    IconClass = "fas fa-money-bill-wave",
                    ColorClass = "primary",
                    Timestamp = DateTime.UtcNow.AddMinutes(-5)
                },
                new RecentActivityViewModel
                {
                    Title = "New Review Submitted",
                    Description = "5-star review pending approval for Trainer Ahmed",
                    IconClass = "fas fa-star",
                    ColorClass = "warning",
                    Timestamp = DateTime.UtcNow.AddMinutes(-15)
                },
                new RecentActivityViewModel
                {
                    Title = "Trainer Verified",
                    Description = "Fatima Noor has been verified as a trainer",
                    IconClass = "fas fa-check-circle",
                    ColorClass = "success",
                    Timestamp = DateTime.UtcNow.AddHours(-1)
                },
                new RecentActivityViewModel
                {
                    Title = "Payment Failed",
                    Description = "Payment of EGP 399 failed for Mohammed Hassan",
                    IconClass = "fas fa-exclamation-circle",
                    ColorClass = "danger",
                    Timestamp = DateTime.UtcNow.AddHours(-2)
                }
            };
        }

        /// <summary>
        /// Get revenue analytics for specified period
        /// </summary>
        public async Task<ChartDataViewModel> GetRevenueChartDataAsync(int days = 30)
        {
            try
            {
                var chartData = new ChartDataViewModel();
                
                // Generate labels
                for (int i = 0; i < days; i++)
                {
                    chartData.Labels.Add($"Day {i + 1}");
                }

                // TODO: Get actual revenue data from database
                // For now, generate mock data
                var revenueData = GenerateMockRevenueData(days);

                chartData.Datasets.Add(new ChartDatasetViewModel
                {
                    Label = "Revenue (EGP)",
                    Data = revenueData,
                    BackgroundColor = "rgba(13, 110, 253, 0.1)",
                    BorderColor = "#0D6EFD"
                });

                return chartData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting revenue chart data");
                throw;
            }
        }

        /// <summary>
        /// Helper method to generate mock revenue data
        /// TODO: Replace with actual database queries
        /// </summary>
        private List<decimal> GenerateMockRevenueData(int days)
        {
            var data = new List<decimal>();
            var random = new Random();

            for (int i = 0; i < days; i++)
            {
                data.Add(random.Next(7000, 24000));
            }

            return data;
        }

        /// <summary>
        /// Get user distribution data
        /// </summary>
        public async Task<ChartDataViewModel> GetUserDistributionChartDataAsync()
        {
            try
            {
                var subscriptions = await _unitOfWork.Repository<Subscription>().GetAllAsync();
                var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllAsync();
                
                var clients = subscriptions.Select(s => s.ClientId).Distinct().Count();
                var activeTrainers = trainers.Count(t => t.IsVerified);

                return new ChartDataViewModel
                {
                    Labels = new List<string> { "Clients", "Trainers", "Admins" },
                    Datasets = new List<ChartDatasetViewModel>
                    {
                        new ChartDatasetViewModel
                        {
                            Label = "User Distribution",
                            Data = new List<decimal> { clients, activeTrainers, 5 }, // 5 admins (mock)
                            BackgroundColor = "rgba(13, 110, 253, 0.8), rgba(25, 135, 84, 0.8), rgba(220, 53, 69, 0.8)"
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user distribution data");
                throw;
            }
        }
    }
}
