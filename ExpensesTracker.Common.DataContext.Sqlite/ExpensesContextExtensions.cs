using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesTracker.Common.DataContext.Sqlite;

public static class ExpensesContextExtensions
{
    public static IServiceCollection AddExpensesContext(this IServiceCollection services, string relativePath = "..")
    {
        string dbPath = Path.Combine(relativePath, "Expenses.db");
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
        var onedrivePath = string.Concat(Environment.GetEnvironmentVariable(useConsumer? "OneDriveConsumer" : "OneDriveCommercial"), "\\_db\\ExpensesTracker\\_data");
        return AddExpensesContext(services, onedrivePath);
    }
}