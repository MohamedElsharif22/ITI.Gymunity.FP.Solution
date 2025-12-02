using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure;
using ITI.Gymunity.FP.Infrastructure._Data;
using ITI.Gymunity.FP.Infrastructure.ExternalServices;
using ITI.Gymunity.FP.Infrastructure.Repositories;
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


            // Register External Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IAuthService, AuthService>();


            return services;
        }
    }
}
