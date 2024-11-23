using ExpensesTracker.Models;

namespace ExpensesTracker.Blazor.Client;

public interface IWalletController
{
    public Task<WalletsListViewModel> Wallets();
}