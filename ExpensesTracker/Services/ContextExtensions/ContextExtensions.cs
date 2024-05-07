using ExpensesTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Services.ContextExtensions
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
            var connectionString = configuration.GetConnectionString("TemplateConnection") ?? throw new InvalidOperationException("Connection string 'TemplateConnection' not found.");
            connectionString = connectionString.Replace("{path}", Environment.GetEnvironmentVariable(useConsumer ? "OneDriveConsumer" : "OneDriveCommercial"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
    }
}
