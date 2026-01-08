using ITI.Gymunity.FP.Application.Contracts;
using ITI.Gymunity.FP.Application.Mapping;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ITI.Gymunity.FP.Application.Configuration;
using ITI.Gymunity.FP.Application.Services.Admin;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;

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

            var stripeSection = configuration.GetSection("Stripe");
            services.Configure<StripeSettings>(options =>
            {
                options.SecretKey = stripeSection["SecretKey"] ?? options.SecretKey;
                options.PublishableKey = stripeSection["PublishableKey"] ?? options.PublishableKey;
                options.WebhookSecret = stripeSection["WebhookSecret"] ?? options.WebhookSecret;
            });

            // ===========================
            // AutoMapper
            // ===========================
            services.AddAutoMapper((opt) => { }, typeof(MappingProfile).Assembly);

            // ===========================
            // Application Services
            // ===========================
            // =========================== Client Services ===========================
            services.AddScoped<ClientProgramsService>();
            services.AddScoped<TrainerProfileService>();
            services.AddScoped<ClientProfileService>();
            services.AddScoped<OnboardingService>();
            services.AddScoped<BodyStateLogService>();
            services.AddScoped<WorkoutLogService>();
            services.AddScoped<ClientTrainersService>();


            // Subscription & Payment
            services.AddScoped<SubscriptionService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<UsersService>();

            //amr start

            // Home Client
            services.AddScoped<IHomeClientService, HomeClientService>();

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

            // ChatBot Service
            services.AddHttpClient();
            services.AddScoped<IChatBotService, ChatBotService>();

            //clients for trainer
            services.AddScoped<IClientService, ClientService>();


            // Register Trainer Review service
            services.AddScoped<ITrainerReviewService, TrainerReviewService>();
            services.AddScoped<IReviewClientService, ReviewClientService>();
            services.AddScoped<IReviewTrainerService, ReviewTrainerService>();
            services.AddScoped<IReviewAdminService, ReviewAdminService>();
            services.AddScoped<IGuestReviewService, GuestReviewService>();

            // ===========================
            // Admin Services
            // ===========================
            services.AddScoped<TrainerAdminService>();
            services.AddScoped<ClientAdminService>();
            services.AddScoped<PaymentAdminService>();
            services.AddScoped<SubscriptionAdminService>();
            services.AddScoped<ProgramAdminService>();

            //amr end

            return services;
        }
    }
}