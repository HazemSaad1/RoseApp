using Microsoft.EntityFrameworkCore;
using RoseApp.API.Extensions;
using RoseApp.API.Hubs;
using RoseApp.API.Services;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Infrastructure.Persistence;
using RoseApp.Infrastructure.Persistence.Seeding;

namespace RoseApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerWithJwt();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // NotificationService 
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            var app = builder.Build();

            app.UseGlobalExceptionMiddleware();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<NotificationHub>("/notifications");

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();

                await DataSeeder.SeedAsync(context);
            }

            app.Run();
        }
    }
}