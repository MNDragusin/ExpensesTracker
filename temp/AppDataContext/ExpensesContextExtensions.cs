using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppDataContext;

public static class ExpensesContextExtensions
{
    //private const string KCloudOsxConnectionString = "Data Source=/Users/{userName}/Library/CloudStorage/OneDrive-Personal/_db/ExpensesTracker/_data/Expenses.db";
    private const string KCloudOsxConnectionString = "Data Source=/Users/{userName}/Library/CloudStorage/OneDrive-Personal/_db/ExpensesTracker/_data/AppData.db";
    private const string KCloudWindowsConnectionString = "Data Source={path}\\_db\\ExpensesTracker\\_data\\Expenses.db";
    
    public static IServiceCollection AddExpensesContext(this IServiceCollection services, string relativePath = "Data Source=..")
    {
        string dbPath = Path.Combine(relativePath, "AppData.db");
        if(!CheckDbConnection(dbPath)){
            return services;
        }
        services.AddDbContext<DataContext>(options =>
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

        services.AddDbContext<DataContext>(options =>
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
            return KCloudOsxConnectionString.Replace("{userName}", Environment.UserName);
        }

        var oneDrivePath = Environment.GetEnvironmentVariable(useConsumer? "OneDriveConsumer" : "OneDriveCommercial");
        return KCloudWindowsConnectionString.Replace("{path}", oneDrivePath);
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