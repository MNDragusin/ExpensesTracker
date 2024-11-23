using ExpensesTracker.Blazor.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Services
{
    public static class ContextExtensions
    {
        public static IServiceCollection AddUserDbContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'TemplateConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }

        public static IServiceCollection AddUserDbContextFromCloud(this IServiceCollection services, ConfigurationManager configuration, bool useConsumer)
        {
            var connectionString = GetConnectionString(configuration, useConsumer);
            if(!IsDatabaseConnected(connectionString)){
                return services;
            }
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }

        private static string GetConnectionString(ConfigurationManager configuration, bool useConsumer)
        {
            string connectionString = string.Empty;
            if (OperatingSystem.IsMacOS())
            {
                connectionString = configuration.GetConnectionString("TemplateMacOSConnection") ?? throw new InvalidOperationException("Connection string 'TemplateConnection' not found.");
                return connectionString.Replace("{username}", Environment.UserName);
            }

            connectionString = configuration.GetConnectionString("TemplateConnection") ?? throw new InvalidOperationException("Connection string 'TemplateConnection' not found.");
            return connectionString.Replace("{path}", Environment.GetEnvironmentVariable(useConsumer ? "OneDriveConsumer" : "OneDriveCommercial"));
        }

        static bool IsDatabaseConnected(string connectionString)
        {
            try
            {
                using (var dbContext = new DbContext(new DbContextOptionsBuilder().UseSqlite(connectionString).Options))
                {
                    dbContext.Database.OpenConnection();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] - Connecting to the database: {ex.Message}");
                return false;
            }
        }
    }
}