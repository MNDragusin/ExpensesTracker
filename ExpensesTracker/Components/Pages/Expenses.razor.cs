using Microsoft.AspNetCore.Components;
using ExpensesTracker.Server.Services;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Components.Account;
using ExpensesTracker.Shared;

namespace ExpensesTracker.Pages;

public class ExpensesBase : ComponentBase
{

    [Inject] IWalletController WalletController { get; set; }
    [Inject] IdentityUserAccessor UserAccessor { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected IEnumerable<WalletEntry> _expenses;
    protected IEnumerable<Category> _categories;
    protected IEnumerable<Wallet> _wallets;
    protected IEnumerable<Label> _labels;

    [CascadingParameter] private HttpContext _httpContext { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var user = await UserAccessor.GetRequiredUserAsync(_httpContext);

        _wallets = await WalletController.GetWallets(user.Id);
        _labels = await WalletController.GetLabels(user.Id);
        _categories = await WalletController.GetCategories(user.Id);

        await LoadEntries();
    }

    private async Task LoadEntries()
    {
        List<WalletEntry> results = new();
        foreach (var wallet in _wallets)
        {
            var result = await WalletController.GetAllExpenses(wallet.Id);
            results.AddRange(result);
        }

        _expenses = results.OrderByDescending(p => p.Date);
    }

    protected void OnDataImported(IEnumerable<WalletEntry> entries)
    {
        _expenses = entries;
    }

    protected async void RemoveEntry(string id)
    {
        bool result = await WalletController.Delete(id);
        if (!result)
        {
            return;
        }
        //This is very very bad....
        await LoadEntries();
        StateHasChanged();
    }
}
