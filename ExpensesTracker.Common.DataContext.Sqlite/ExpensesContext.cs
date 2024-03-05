using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Common.DataContext.Sqlite;

public class ExpensesContext : DbContext
{
    public ExpensesContext()
    {

    }

    public ExpensesContext(DbContextOptions<ExpensesContext> options) : base(options)
    {

    }

    public virtual DbSet<Owner> Owners { get; set; }
    public virtual DbSet<Wallet> Wallets { get; set; }
    public virtual DbSet<WalletEntry> WalletEntries { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Label> Labels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string path = Path.Combine("..", "Expenses.db");
        optionsBuilder.UseSqlite($"Filename={path}");
    }
}