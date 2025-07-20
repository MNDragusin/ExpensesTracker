using ExpensesTracker.Common.Entities;

namespace ExpensesTracker.Services
{
    public interface IWalletRepository
    {
        public Task<Wallet> AddNewWalletAsync(Wallet newWallet);
        public Task<WalletEntry> AddNewEntryAsync(WalletEntry newEntry);
        


        public Task<Wallet> GetWalletAsync(string walletId);
        public Task<List<WalletEntry>> GetAllEntriesAsync(string walletId);
    }
}
