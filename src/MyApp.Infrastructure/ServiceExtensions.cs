using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Add this for IConfiguration
using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Core.Services;
using MyApp.Application.Interfaces;
using MyApp.Application.Services;
using MyApp.Domain.Core.Repositories;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Repositories;
using MyApp.Infrastructure.Services;

namespace MyApp.Infrastructure
{
    public static class ServiceExtensions
    {

        public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {            
            // Retrieve the connection string from environment variables or appsettings
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
                                    ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MyAppDbContext>(options =>
                options.UseNpgsql(connectionString, // Use the correct connection string here
                x => x.MigrationsAssembly("MyApp.Infrastructure")));

            services.AddScoped(typeof(IBaseRepositoryAsync<>), typeof(BaseRepositoryAsync<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IPhotos, Photos>();

            services.AddSignalR();
            services.AddTransient<IChatService, ChatService>();

            var key = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY") 
                                    ?? configuration["OpenAI:ApiKey"];
            
            services.AddHttpClient("OpenAI", client =>
            {
                client.BaseAddress = new Uri("https://api.openai.com/v1/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
            });
        }

        public static void MigrateDatabase(this IServiceProvider serviceProvider)
        {
            var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<MyAppDbContext>>();

            using (var dbContext = new MyAppDbContext(dbContextOptions))
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
