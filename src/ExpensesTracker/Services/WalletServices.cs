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
            var result = await _context.Wallets.AddAsync(newWallet);
            
            if(result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return null;
            }
            
            await _context.SaveChangesAsync();
            return result.Entity;
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

        public async Task<Wallet> GetWalletAsync(string walletId)
        {
            return await _context.Wallets.FindAsync(walletId);
        }

        public async Task<List<WalletEntry>> GetAllEntriesAsync(string walletId)
        {
            var result = await _context.WalletEntries
                .Where(x => x.WalletId == walletId)
                .ToListAsync();

            return result;
        }
    }
}
