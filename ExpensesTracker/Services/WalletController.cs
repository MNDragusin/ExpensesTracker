using ExpensesTracker.Common.DataContext.Sqlite;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Server.Services;

public class WalletController : IWalletController
{
    private ExpensesContext _context;
    private IWalletController _walletControllerImplementation;

    public WalletController(ExpensesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WalletEntry>> GetAllExpenses(string walletId)
    {
        return await _context.WalletEntries.Where(entry => entry.WalletId == walletId).ToListAsync();
    }

    public async Task<WalletEntry> GetEntry(string entryId)
    {
        return await _context.WalletEntries.FindAsync(entryId);
    }

    public async Task<WalletEntry> UpdateEntry(WalletEntry entry)
    {
        var result = _context.WalletEntries.Update(entry);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(string id)
    {
        var entry = await GetEntry(id);
        _context.WalletEntries.Remove(entry);
        return await _context.SaveChangesAsync() != 0;
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
    
    public async Task<WalletEntry> AddNewEntry(WalletEntry walletEntry)
    {
        var result = await _context.WalletEntries.AddAsync(walletEntry);
        await _context.SaveChangesAsync();
        
        return result.Entity;
    }

    public async Task<Label> AddNewLabel(Label newLabel)
    {
        var result = await _context.Labels.AddAsync(newLabel);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<Category> AddNewCategory(Category newCategory)
    {
        var result = await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<Wallet> AddNewWallet(Wallet newWallet)
    {
        var result = await _context.Wallets.AddAsync(newWallet);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<string> MigrateToNewId(string oldId, string newId)
    {
        try
        {
            var owner = _context.Owners.FirstOrDefault(o => o.OwnerId == oldId);
            Owner newOwner = new Owner()
            {
                OwnerId = newId,
                Name = owner.Name,
                Wallets = owner.Wallets,
                Categories = owner.Categories,
                Labels = owner.Labels
            };
            
            _context.Owners.Remove(owner);
            await _context.Owners.AddAsync(newOwner);
            
            foreach (var wallet in _context.Wallets.Where(w => w.OwnerId == oldId))
            {
                wallet.OwnerId = newId;
            }
            
            foreach (var category in _context.Categories.Where(c=>c.OwnerId == oldId))
            {
                category.OwnerId = newId;
            }

            foreach (var label in _context.Labels.Where(l=>l.OwnerId == oldId))
            {
                label.OwnerId = newId;
            }
            
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return "Migration Complete";
    }
}