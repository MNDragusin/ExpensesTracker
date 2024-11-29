using ExpensesTracker.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Components;

public partial class WalletNavBar : ComponentBase, IDisposable
{
    [Inject]
    public required WalletState WalletState { get; set; }

    protected override void OnInitialized()
    {
        WalletState.StateChanged += this.StateHasChanged;
        base.OnInitialized();
    }

    public void Dispose()
    {
        WalletState.StateChanged -= this.StateHasChanged;
    }
}