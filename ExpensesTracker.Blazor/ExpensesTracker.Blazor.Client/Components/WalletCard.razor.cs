using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Components;

public partial class WalletCard : ComponentBase
{
    [Parameter]
    public required string WalletId { get; set; }
    [Parameter]
    public required string WalletName { get; set; }
    [Parameter]
    public required float TotalAmount { get; set; }
    [Parameter]
    public required string ColorCode { get; set; }
    
    [Parameter]
    public EventCallback<string> OnWalletSelectedCallback { get; set; }
    
    public string WalletHref { get; set; }

    protected override void OnParametersSet()
    {
        WalletHref = $"/wallet/{WalletId}";
        base.OnParametersSet();
    }

    private void OnWalletSelected()
    {
        OnWalletSelectedCallback.InvokeAsync(WalletId);
    }
}