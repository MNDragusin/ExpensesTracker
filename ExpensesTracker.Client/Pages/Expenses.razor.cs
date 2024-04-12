using Microsoft.AspNetCore.Components;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace ExpensesTracker.Pages;

public class ExpensesBase : ComponentBase
{

    [Inject] IWalletController WalletController { get; set; }
    [Inject] AuthenticationStateProvider? _persistentAuthenticationStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected IEnumerable<WalletEntry> _expenses;
    protected IEnumerable<Category> _categories;
    protected IEnumerable<Wallet> _wallets;
    protected IEnumerable<Label> _labels;

    protected bool ShowLoading = true;

    protected override async Task OnInitializedAsync()
    {
        ShowLoading = true;
        AuthenticationState state = await _persistentAuthenticationStateProvider!.GetAuthenticationStateAsync();

        if (!state.User.Identity.IsAuthenticated)
        {
            return;
        }

        string claim = string.Empty;
        claim = state.User.Claims.FirstOrDefault().Value;

        _wallets = await WalletController.GetWallets(claim);
        _labels = await WalletController.GetLabels(claim);
        _categories = await WalletController.GetCategories(claim);

        await LoadEntries();
        ShowLoading = false;
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
