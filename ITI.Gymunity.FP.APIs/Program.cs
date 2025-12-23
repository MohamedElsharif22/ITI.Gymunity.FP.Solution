using ITI.Gymunity.FP.APIs.Middleware;
using ITI.Gymunity.FP.APIs.Extensions;
using ITI.Gymunity.FP.APIs.Hubs;
using ITI.Gymunity.FP.APIs.Middlewares;
using ITI.Gymunity.FP.Application.DependencyInjection;
using ITI.Gymunity.FP.Infrastructure._Data;
using ITI.Gymunity.FP.Infrastructure.Dependancy_Injection;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                configure.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "Gymunity APIs", Version = "v1" });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ITI.Gymunity.FP.APIs.xml"));
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token here. Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Add SignalR
            builder.Services.AddSignalR();

            // Add CORS for SignalR
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("wepPolicy", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(origin => true);
                });
            });

            builder.Services.AddDbContextServices(builder.Configuration);

            builder.Services.AddInfrastructureServices();

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddAuthenticationServices(builder.Configuration);
            builder.Services.AddMemoryCache();
            //Confiure Api Invalid Model State Response
            builder.Services.AddApiInvalidModelStateConfiguration();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            await app.MiagrateAndSeedDatabasesAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.MapControllers();

            // Map SignalR Hubs
            app.MapHub<ChatHub>("/hubs/chat");
            app.MapHub<NotificationHub>("/hubs/notifications");

            app.UseCors("wepPolicy");

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseMiddleware<WebhookSecurityMiddleware>();

            app.Run();
        }
    }
}
