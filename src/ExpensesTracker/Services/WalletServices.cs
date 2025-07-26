using ExpensesTracker.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Services
{
    public class WalletServices : IWalletRepository
    {
        private readonly DataContext.DataContext _context;
        public WalletServices(DataContext.DataContext context)
        {
            _context = context;
        }

        public async Task<Wallet> AddNewWalletAsync(Wallet newWallet)
        {
            try
            {
                var result = await _context.Wallets.AddAsync(newWallet);
            
                if(result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    return null;
                }
            
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<WalletEntry> AddNewEntryAsync(WalletEntry newEntry)
        {
            var result = await _context.WalletEntries.AddAsync(newEntry);
           
            
            if(result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public Task<List<Wallet>> GetWalletsAsync(string ownerId)
        {
            return _context.Wallets
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<List<WalletEntry>> GetAllEntriesAsync(string walletId)
        {
            var result = await _context.WalletEntries
                .Where(x => x.WalletId == walletId)
                .ToListAsync();

            return result;
        }

        public Task<List<Category>> GetCategoriesAsync(string ownerId)
        { 
            var result = _context.Categories
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
            return result;
        }

        public Task<List<Label>> GetLabelsAsync(string ownerId)
        {
            var result = _context.Labels
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
            return result;
        }

        public async Task<Category> AddNewCategoryAsync(Category cat)
        {
            var result = await _context.Categories.AddAsync(cat);
            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Label> AddNewLabelAsync(Label label)
        {
            var result = await _context.Labels.AddAsync(label);
            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
