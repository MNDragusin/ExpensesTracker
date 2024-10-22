
using ExpensesTracker.Common.EntityModel.Sqlite;

namespace ExpensesTracker.Services;

public interface IWalletServices
{
    public Task<IEnumerable<WalletEntry>?> GetAllExpenses(string walletId);
    public Task<WalletEntry?> GetEntry(string entryId);
    public Task<WalletEntry?> UpdateEntry(WalletEntry entry);
    public Task<bool> Delete(string id);
    public Task<IEnumerable<Wallet>?> GetWallets(string ownerId);
    public Task<IEnumerable<Category>?> GetCategories(string ownerId);
    public Task<IEnumerable<Label>?> GetLabels(string ownerId);

    public Task<WalletEntry?> AddNewEntry(WalletEntry walletEntry);

    public Task<Label?> AddNewLabel(Label newLabel);
    public Task<Category?> AddNewCategory(Category newCategory);
    public Task<Wallet?> AddNewWallet(Wallet newWallet);
    public Task<bool> DeletWallet(string walletId);

    //public Task<string> MigrateToNewId(string oldId, string newId);
}