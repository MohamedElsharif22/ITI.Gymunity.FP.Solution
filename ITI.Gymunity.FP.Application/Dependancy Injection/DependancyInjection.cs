using ITI.Gymunity.FP.Application.Configuration;
using ITI.Gymunity.FP.Application.Mapping;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using ITI.Gymunity.FP.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}