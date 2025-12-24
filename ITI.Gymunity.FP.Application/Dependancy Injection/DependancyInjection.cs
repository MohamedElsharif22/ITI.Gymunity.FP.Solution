using ITI.Gymunity.FP.Application.Contracts;
using ITI.Gymunity.FP.Application.Mapping;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ITI.Gymunity.FP.Application.Configuration;
using ITI.Gymunity.FP.Application.Services.Admin;

namespace ITI.Gymunity.FP.Application.DependencyInjection
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ===========================
            // Configuration Settings
            // ===========================
            var paypalSection = configuration.GetSection("PayPal");
            services.Configure<PayPalSettings>(options =>
            {
                options.Mode = paypalSection["Mode"] ?? options.Mode;
                options.ClientId = paypalSection["ClientId"] ?? options.ClientId;
                options.ClientSecret = paypalSection["ClientSecret"] ?? options.ClientSecret;
                options.ReturnUrl = paypalSection["ReturnUrl"] ?? options.ReturnUrl;
                options.CancelUrl = paypalSection["CancelUrl"] ?? options.CancelUrl;
            });

            // ===========================
            // AutoMapper
            // ===========================
            services.AddAutoMapper((opt) => { }, typeof(MappingProfile).Assembly);

            // ===========================
            // Application Services
            // ===========================
            services.AddScoped<TrainerProfileService>();
            services.AddScoped<ClientProfileService>();
            services.AddScoped<OnboardingService>();
            services.AddScoped<BodyStateLogService>();
            services.AddScoped<WorkoutLogService>();

            // Subscription & Payment
            services.AddScoped<SubscriptionService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<WebhookService>();
            services.AddScoped<PayPalService>();
            services.AddScoped<UsersService>();

            //amr start

            // Home Client
            IServiceCollection serviceCollection = services.AddScoped<IHomeClientService, HomeClientService>();

            // Day - Week - Program services
            services.AddScoped<IDayExerciseService, DayExerciseService>();
            services.AddScoped<IDayService, DayService>();
            services.AddScoped<IWeekService, WeekService>();
            services.AddScoped<IProgramService, ProgramService>();
            services.AddScoped<IProgramManagerService, ProgramManagerService>();

            // Exercise Library
            services.AddScoped<IExerciseLibraryService, ExerciseLibraryService>();

            

            //packages

            services.AddScoped<IPackageService, PackageService>();

            //clients for trainer

            services.AddScoped<IClientService, ClientService>();

            // Register Trainer Review service
            services.AddScoped<ITrainerReviewService, TrainerReviewService>();
            services.AddScoped<IReviewClientService, ReviewClientService>();
            services.AddScoped<IReviewTrainerService, ReviewTrainerService>();
            services.AddScoped<IReviewAdminService, ReviewAdminService>();
            services.AddScoped<IGuestReviewService, GuestReviewService>();

            //amr end




            return services;
        }
    }
}