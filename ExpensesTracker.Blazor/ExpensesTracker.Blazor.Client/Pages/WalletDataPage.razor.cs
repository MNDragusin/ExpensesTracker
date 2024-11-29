using ExpensesTracker.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Pages;

public partial class WalletDataPage : ComponentBase, IDisposable
{
   [Inject]
   private WalletState _currentWalletState { get; set; }
   [Parameter]
   public required string WalletId { get; set; }

   protected override void OnInitialized()
   {
      _currentWalletState.StateChanged += this.StateHasChanged;
      base.OnInitialized();
   }

   public void Dispose()
   {
      _currentWalletState.StateChanged += this.StateHasChanged;
   }
}