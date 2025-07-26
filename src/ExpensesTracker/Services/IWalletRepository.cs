using ExpensesTracker.Common.Dtos;
using ExpensesTracker.Common.Entities;

namespace ExpensesTracker.Services
{
    public interface IWalletRepository
    {
        public Task<Wallet> AddNewWalletAsync(Wallet newWallet);
        public Task<WalletEntry> AddNewEntryAsync(WalletEntry newEntry);
        
        
        public Task<List<Wallet>> GetWalletsAsync(string ownerId);
        public Task<List<WalletEntry>> GetAllEntriesAsync(string walletId);
        public Task<List<Category>> GetCategoriesAsync(string ownerId);
        public Task<List<Label>> GetLabelsAsync(string ownerId);
        public Task<Category> AddNewCategoryAsync(Category cat);
        public Task<Label> AddNewLabelAsync(Label label);
    }
}
