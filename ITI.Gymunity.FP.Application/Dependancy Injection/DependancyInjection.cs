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
<<<<<<< HEAD
            services.AddAutoMapper((opt) => { },typeof(MappingProfile).Assembly);
=======
            services.AddAutoMapper((opt) => { }, typeof(MappingProfile).Assembly);
>>>>>>> main
            services.AddScoped<TrainerProfileService>();

            return services;
        }
    }
}
