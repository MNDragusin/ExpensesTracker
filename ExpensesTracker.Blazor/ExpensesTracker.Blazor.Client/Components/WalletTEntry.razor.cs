using ExpensesTracker.Models;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Components;

public partial class WalletTEntry : ComponentBase
{
    [Parameter]
    public Entry Data { get; set; }
}