using System.ComponentModel;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.AspNetCore.Components;
using ExpensesTracker.Shared;
using ExpensesTracker.Components.Account;
using MudBlazor;

namespace ExpensesTracker;

public class WalletsAndDataBase : ComponentBase
{
    [CascadingParameter] private HttpContext _httpContext { get; set; } = default!;
    [Inject] IdentityUserAccessor UserAccessor {get;set;}
    [Inject] IWalletController WalletController {get;set;}

    protected IEnumerable<Wallet> _wallets;
    protected IEnumerable<Category> _categories;
    protected IEnumerable<Label> _labels;

    protected override async Task OnInitializedAsync()
    {
        var user = await UserAccessor.GetRequiredUserAsync(_httpContext);
        var ownerId = user.Id;
        
        _wallets = (await WalletController.GetWallets(ownerId)).OrderBy(w => w.Name);
        _categories = (await WalletController.GetCategories(ownerId)).OrderBy(c => c.Name);
        _labels = (await WalletController.GetLabels(ownerId)).OrderBy(l => l.Name);
    }
    
    protected async void OnNewLabel(string newLabelName)
    {
        var user = await UserAccessor.GetRequiredUserAsync(_httpContext);

        Label label = new Label() { Name = newLabelName, OwnerId = user.Id };
        var restul = await WalletController.AddNewLabel(label);
        _labels = _labels.Append(restul).OrderBy(l => l.Name);

        StateHasChanged();
    }

    protected async void OnNewWallet(string newWalletName)
    {
        var user = await UserAccessor.GetRequiredUserAsync(_httpContext);

        Wallet wallet = new() { Name = newWalletName, OwnerId = user.Id };
        var restul = await WalletController.AddNewWallet(wallet);
        _wallets = _wallets.Append(restul).OrderBy(l => l.Name);

        StateHasChanged();
    }

    protected async void OnNewCategory(string newCategoryName)
    {
        var user = await UserAccessor.GetRequiredUserAsync(_httpContext);

        Category category = new() { Name = newCategoryName, OwnerId = user.Id };
        var restul = await WalletController.AddNewCategory(category);
        _categories = _categories.Append(restul).OrderBy(l => l.Name);

        StateHasChanged();
    }
}
