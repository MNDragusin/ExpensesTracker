using ExpensesTracker.Common.DataContext.Sqlite;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Services;

public class WalletRepository : IWalletRepository
{
    private ExpensesContext _context;

    public WalletRepository(ExpensesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WalletEntry>?> GetAllExpenses(string walletId)
    {
        return await _context.WalletEntries.Where(entry => entry.WalletId == walletId).ToListAsync();
    }

    public async Task<WalletEntry?> GetEntry(string entryId)
    {
        return await _context.WalletEntries.FindAsync(entryId);
    }

    public async Task<WalletEntry?> UpdateEntry(WalletEntry entry)
    {
        var result = _context.WalletEntries.Update(entry);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(string id)
    {
        var entry = await GetEntry(id);
        if(entry is null){
            return false;
        }

        _context.WalletEntries.Remove(entry);
        return await _context.SaveChangesAsync() != 0;
    }

    public async Task<IEnumerable<Wallet>?> GetWallets(string ownerId)
    {
        return await _context.Wallets.Where(wallet => wallet.OwnerId == ownerId).ToListAsync();
    }

    public async Task<IEnumerable<Category>?> GetCategories(string ownerId)
    {
        return await _context.Categories.Where(category => category.OwnerId == ownerId).ToListAsync();
    }

    public async Task<IEnumerable<Label>?> GetLabels(string ownerId)
    {
        return await _context.Labels.Where(label => label.OwnerId == ownerId).ToListAsync();
    }
    
    public async Task<WalletEntry?> AddNewEntry(WalletEntry walletEntry)
    {
        walletEntry.EntryId = null;
        var result = await _context.WalletEntries.AddAsync(walletEntry);
        await _context.SaveChangesAsync();
        
        return result.Entity;
    }

    public async Task<Label?> AddNewLabel(Label newLabel)
    {
        newLabel.Id = null;
        var result = await _context.Labels.AddAsync(newLabel);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<Category?> AddNewCategory(Category newCategory)
    {
        newCategory.Id = null;
        var result = await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<Wallet?> AddNewWallet(Wallet newWallet)
    {
        newWallet.Id = null;
        var result = await _context.Wallets.AddAsync(newWallet);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<bool> DeletWallet(string walletId)
    {
        var walletToDelete = await _context.Wallets.FirstOrDefaultAsync(w=>w.Id == walletId);
        if(walletToDelete is null){
            return false;
        }
        
        _context.Wallets.Remove(walletToDelete);

        return await _context.SaveChangesAsync() == 0 ? false: true;
    }
}