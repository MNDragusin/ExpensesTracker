using ExpensesTracker.Common.DataContext.Sqlite;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Server.Services;

public class WalletController : IWalletController
{
    private ExpensesContext _context;
    public WalletController(ExpensesContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<WalletEntry>> GetAllExpenses(string walletId)
    {
        return await _context.WalletEntries.Where(entry => entry.WalletId == walletId).ToListAsync();
    }

    public async Task<IEnumerable<Wallet>> GetWallets(string ownerId)
    {
        return await _context.Wallets.Where(wallet => wallet.OwnerId == ownerId).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategories(string ownerId)
    {
        return await _context.Categories.Where(category => category.OwnerId == ownerId).ToListAsync();
    }

    public async Task<IEnumerable<Label>> GetLabels(string ownerId)
    {
        return await _context.Labels.Where(label => label.OwnerId == ownerId).ToListAsync();
    }

    private IEnumerable<WalletEntry> GenerateMockData(int count)
    {
        List<WalletEntry> mockData = new List<WalletEntry>();
        for (int i = 0; i < count; i++)
        {
            mockData.Add(new WalletEntry()
            {
                EntryId= i.ToString(),
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                WalletId = "mdWallet",
                CategoryId = "food",
                LabelId = "Rewe",
                Amount = i + 10
            });
        }

        return mockData;
    }
}