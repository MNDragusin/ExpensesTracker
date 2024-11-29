using ExpensesTracker.Blazor.Client.Services;
using ExpensesTracker.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Pages;

public partial class WalletDataPage : ComponentBase, IDisposable
{
   [Inject]
   private WalletState _currentWalletState { get; set; }
   [Inject]
   private IWalletController _walletController { get; set; }
   [Parameter]
   public required string WalletId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //this is ugly us shit....
        if (!_currentWalletState.CurrentWallet.HasData || _currentWalletState.CurrentWallet.WalletId != WalletId)
        {
            var wallets = await _walletController.Wallets();
            _currentWalletState.CurrentWallet = wallets.Wallets.FirstOrDefault(w => w.WalletId == WalletId);
        }

        _currentWalletState.StateChanged += this.StateHasChanged;
        await base.OnInitializedAsync();
    }

   public void Dispose()
   {
        _currentWalletState.StateChanged += this.StateHasChanged;
   }
}