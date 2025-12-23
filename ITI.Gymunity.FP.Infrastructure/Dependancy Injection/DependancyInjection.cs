using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.Contracts.Services;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Infrastructure._Data;
using ITI.Gymunity.FP.Infrastructure.ExternalServices;
using ITI.Gymunity.FP.Infrastructure.Repositories.ClientRepositories;
using ITI.Gymunity.FP.Infrastructure.Repositories;

namespace ITI.Gymunity.FP.Infrastructure.Dependancy_Injection
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration _configuration)
        {

            // Adding Authintication Schema Bearer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty)),
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero,
                };

            });

            return services;
        }
        public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Configure Context Services
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>();



            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register Repositories 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITrainerProfileRepository, TrainerProfileRepository>();
            services.AddScoped<IClientProfileRepository, ClientProfileRepository>();
            services.AddScoped<IWorkoutLogRepository, WorkoutLogRepository>();


            services.AddScoped<ITrainerProfileRepository, TrainerProfileRepository>();

            //amr start
            // Register Repositories 
            services.AddScoped<IExerciseLibraryRepository, ExerciseLibraryRepository>();
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IWeekRepository, WeekRepository>();
            services.AddScoped<IDayRepository, DayRepository>();
            services.AddScoped<IDayExerciseRepository, DayExerciseRepository>();
           
            //services.AddScoped<IClientRepository, ClientRepository>();

            //packages

            services.AddScoped<IPackageRepository, PackageRepository>();

            // Register trainer review repo
            services.AddScoped<ITrainerReviewRepository, TrainerReviewRepository>();
            
            // ensure repositories
            services.AddScoped<IReviewClientRepository, ReviewClientRepository>();
            services.AddScoped<IReviewTrainerRepository, ReviewTrainerRepository>();
            services.AddScoped<IReviewAdminRepository, ReviewAdminRepository>();
            services.AddScoped<IGuestReviewRepository, GuestReviewRepository>();

            //amr end

            // Register External Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IAuthService, AuthService>();
            //services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IImageUrlResolver, ImageUrlResolver>();

            // Register SignalR Services
            services.AddSingleton<ISignalRConnectionManager, SignalRConnectionManager>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<INotificationService, NotificationService>();




            return services;
        }
    }
}
