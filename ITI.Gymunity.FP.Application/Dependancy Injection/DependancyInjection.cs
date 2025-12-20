using ITI.Gymunity.FP.Infrastructure.Contracts.ExternalServices;
using ITI.Gymunity.FP.Infrastructure.Mapping;
using ITI.Gymunity.FP.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ITI.Gymunity.FP.Application.DependencyInjection
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your application services here => سجل ال Services الخاصة بالتطبيق هنا

            // Auto Mapper Configurations
            services.AddAutoMapper((opt) => { }, typeof(MappingProfile).Assembly);
            services.AddScoped<TrainerProfileService>();
            services.AddScoped<SubscriptionService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<WebhookService>();

            // Email implementation lives in the Infrastructure project and should be registered there.
            // e.g. in ITI.Gymunity.FP.Infrastructure.Dependancy_Injection.AddInfrastructureServices
            // services.AddScoped<IEmailService, EmailService>();
            // services.AddScoped<EmailTemplateService>();

            return services;
        }
    }
}

