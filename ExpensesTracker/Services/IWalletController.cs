
using ExpensesTracker.Common.EntityModel.Sqlite;

namespace ExpensesTracker.Server.Services;

public interface IWalletController
{
    public Task<IEnumerable<WalletEntry>> GetAllExpenses(string walletId);
    public Task<IEnumerable<Wallet>> GetWallets(string ownerId);
    public Task<IEnumerable<Category>> GetCategories(string ownerId);
    public Task<IEnumerable<Label>> GetLabels(string ownerId);
}