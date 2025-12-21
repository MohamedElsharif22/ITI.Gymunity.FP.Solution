using ITI.Gymunity.FP.Application.Contracts;
using ITI.Gymunity.FP.Application.Mapping;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.Services.ClientServices;
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
            services.AddScoped<ClientProfileService>();
            services.AddScoped<OnboardingService>();
            services.AddScoped<BodyStateLogService>();
            services.AddScoped<WorkoutLogService>();
            services.AddScoped<SubscriptionService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<WebhookService>();

            // Email implementation lives in the Infrastructure project and should be registered there.
            // e.g. in ITI.Gymunity.FP.Infrastructure.Dependancy_Injection.AddInfrastructureServices
            // services.AddScoped<IEmailService, EmailService>();
            // services.AddScoped<EmailTemplateService>();

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

