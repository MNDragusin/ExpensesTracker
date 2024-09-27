using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesTracker.Common.DataContext.Sqlite;

public static class ExpensesContextExtensions
{
    private const string kCloudOSXConnectionString = "Data Source=/Users/{userName}/Library/CloudStorage/OneDrive-Personal/_db/ExpensesTracker/_data/Expenses.db";
    private const string kCloudWindowsConnectionString = "Data Source={path}\\_db\\ExpensesTracker\\_data\\Expenses.db";
    
    public static IServiceCollection AddExpensesContext(this IServiceCollection services, string relativePath = "..")
    {
        string dbPath = Path.Combine(relativePath, "Expenses.db");
        if(!CheckDbConnection(dbPath)){
            return services;
        }
        services.AddDbContext<ExpensesContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
            options.LogTo(Console.WriteLine, new[]
            {
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting
            });
        });

        return services;
    }

    public static IServiceCollection AddExpensesContextFromCloud(this IServiceCollection services, bool useConsumer)
    {
        string onedrivePath = GetCloudPath(useConsumer); 
        
        if(!CheckDbConnection(onedrivePath)){
            return services;
        }

        services.AddDbContext<ExpensesContext>(options =>
        {
            options.UseSqlite(onedrivePath);
            options.LogTo(Console.WriteLine, new[]
            {
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting
            });
        });

        return services;
    }

    private static string GetCloudPath(bool useConsumer){

        if(OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst()){
            return kCloudOSXConnectionString.Replace("{userName}", Environment.UserName);
        }

        var oneDrivePath = Environment.GetEnvironmentVariable(useConsumer? "OneDriveConsumer" : "OneDriveCommercial");
        return kCloudWindowsConnectionString.Replace("{path}", oneDrivePath);
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
            Console.WriteLine($"[Error] - Connecting to the database: {ex.Message}");
            return false;
        }
    }
}