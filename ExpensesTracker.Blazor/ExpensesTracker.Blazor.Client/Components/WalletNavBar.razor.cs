using ExpensesTracker.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Components;

public partial class WalletNavBar : ComponentBase
{
    [Inject]
    public required WalletState WalletState { get; set; }

    [Parameter]
    public EventCallback OnShowNewEntryCallback { get; set; }
    protected override void OnInitialized()
    {
        //WalletState.StateChanged += this.StateHasChanged;
        base.OnInitialized();
    }

    protected void OnNewEntryBtn()
    {
        OnShowNewEntryCallback.InvokeAsync();
        Console.WriteLine("onNewEntryBtn");
    }
}