using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppDataContext;

public static class ExpensesContextExtensions
{
    private const string KdbName = "AppData.db"; //"Expenses.db"
    private const string KCloudOsxConnectionString = $"Data Source=/Users/[userName]/Library/CloudStorage/OneDrive-Personal/_db/ExpensesTracker/_data/{KdbName}";
    private const string KCloudWindowsConnectionString = $"Data Source=[path]\\_db\\ExpensesTracker\\_data\\{KdbName}";
    private const string KsqliteConnectionString = "Data Source=";
    public static IServiceCollection AddExpensesContext(this IServiceCollection services, string relativePath = "..")
    {
        string dbPath = KsqliteConnectionString;
        dbPath += Path.Combine(KsqliteConnectionString, relativePath, KdbName);
        return services.AddDbContextFrom(dbPath);
    }

    public static IServiceCollection AddExpensesContextFromCloud(this IServiceCollection services, bool useConsumer)
    {
        string onedrivePath = GetCloudPath(useConsumer); 
        return services.AddDbContextFrom(onedrivePath);
    }

    private static IServiceCollection AddDbContextFrom(this IServiceCollection services, string path)
    {
        if (!CheckDbConnection(path))
        {
            return services;
        }

        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(path);
            options.LogTo(Console.WriteLine, new[]
            {
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting
            });
        });

        return services;
    }

    private static string GetCloudPath(bool useConsumer){

        if(OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst()){
            return KCloudOsxConnectionString.Replace("[userName]", Environment.UserName);
        }

        var oneDrivePath = Environment.GetEnvironmentVariable(useConsumer? "OneDriveConsumer" : "OneDriveCommercial");
        return KCloudWindowsConnectionString.Replace("[path]", oneDrivePath);
    }

    static bool CheckDbConnection(string connectionString)
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
            Console.WriteLine($"[Error][ExpensesContextExtensions] - Connecting to the database: {ex.Message}");
            return false;
        }
    }
}