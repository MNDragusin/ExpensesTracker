using ExpensesTracker.Models;

namespace ExpensesTracker.Blazor.Client.Shared;

public class WalletState
{
    public event Action? StateChanged;
    private WalletViewModel _currentWallet;
    public WalletViewModel CurrentWallet
    {
        get => _currentWallet;
        set
        {
            _currentWallet = value;
            StateChanged?.Invoke();
        } 
    }
}