using ITI.Gymunity.FP.Application.Mapping;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Dependancy_Injection
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your application services here => سجل ال Services الخاصة بالتطبيق هنا

            // Auto Mapper Configurations
            services.AddAutoMapper((opt) => { },typeof(MappingProfile).Assembly);
            services.AddScoped<TrainerProfileService>();

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

            // Chat
            services.AddScoped<IChatService, ChatService>();

            //packages

            services.AddScoped<IPackageService, PackageService>();

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
