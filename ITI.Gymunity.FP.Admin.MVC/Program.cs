using ITI.Gymunity.FP.Application.DependencyInjection;
using ITI.Gymunity.FP.Admin.MVC.Services;
using ITI.Gymunity.FP.Admin.MVC.Hubs;
using ITI.Gymunity.FP.Infrastructure.Dependancy_Injection;
using ITI.Gymunity.FP.Application.Contracts.Admin;
using ITI.Gymunity.FP.Application.Services.Admin;
using ITI.Gymunity.FP.Infrastructure.ExternalServices;

namespace ITI.Gymunity.FP.Admin.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add SignalR
            builder.Services.AddSignalR();

            // Add CORS for SignalR
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("adminSignalRPolicy", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(origin => true);
                });
            });

            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Gymunity.Admin.Cookie";
                    config.LoginPath = "/Auth/Login";
                });
            builder.Services.AddDbContextServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices();

            // Add Dashboard Service
            builder.Services.AddScoped<DashboardStatisticsService>();

            // Add Analytics Service
            builder.Services.AddScoped<AnalyticsService>();

            // ✅ Register Admin Services (required for notification handlers)
            builder.Services.AddScoped<PaymentAdminService>();
            builder.Services.AddScoped<SubscriptionAdminService>();
            builder.Services.AddScoped<TrainerAdminService>();
            builder.Services.AddScoped<UserManagementService>();
            builder.Services.AddScoped<IUserManagementService, UserManagementService>();
            
            // ✅ Register AccountService (from Infrastructure layer)
            builder.Services.AddScoped<AccountService>();

            // Add Admin Notification Services
            builder.Services.AddScoped<AdminUserResolverService>();
            builder.Services.AddScoped<IAdminNotificationService, AdminNotificationService>();

            // ✅ Register All Admin Notification Handlers
            // These services subscribe to events from business logic services
            // and send real-time notifications to admins via SignalR
            builder.Services.AddScoped<AccountNotificationService>();      // User registrations
            builder.Services.AddScoped<PaymentNotificationService>();      // Payment events
            builder.Services.AddScoped<SubscriptionNotificationService>(); // Subscription events
            builder.Services.AddScoped<UserNotificationService>();         // User management events
            builder.Services.AddScoped<TrainerNotificationService>();      // Trainer management events

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.MapStaticAssets();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable CORS before mapping SignalR hubs
            app.UseCors("adminSignalRPolicy");

            // Map SignalR hub
            app.MapHub<AdminNotificationHub>("/hubs/admin-notifications");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
