
using ExpensesTracker.Common.EntityModel.Sqlite;

namespace ExpensesTracker.Server.Services;

public interface IWalletController
{
    public Task<IEnumerable<WalletEntry>> GetAllExpenses();
}