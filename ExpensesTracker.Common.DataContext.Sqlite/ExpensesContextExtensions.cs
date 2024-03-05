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
}