using Microsoft.AspNetCore.Components;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Components.Account;
using ExpensesTracker.Shared;

namespace ExpensesTracker.Pages;

public class AddNewExpenseBase : ComponentBase
{
    [Inject] IWalletController WalletController { get; set; }
    [Inject] IdentityUserAccessor UserAccessor {get;set;}
    [Inject] NavigationManager NavigationManager { get; set; }

    [SupplyParameterFromForm] protected WalletEntry? NewEntry { get; set; } = null;
    [CascadingParameter] protected HttpContext HttpContext { get; set; } = default!;
    
    protected IEnumerable<Wallet>? _wallets;
    protected IEnumerable<Label>? _labels;
    protected IEnumerable<Category>? _categories;

    protected string CategoryIdSelected {get;set;} = string.Empty;
    protected string _buttonName = "Add new";
    private bool _isEditMode = false;
    
    [Parameter] public string Id { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (!NavigationManager.Uri.Contains("/EditEntry/"))
        {
            NewEntry ??= new() { Date = DateOnly.FromDateTime(DateTime.Now) };
        }

        var user = await UserAccessor.GetRequiredUserAsync(HttpContext);
        
        _wallets = (await WalletController.GetWallets(user.Id)).OrderBy(w=>w.Name);
        _labels = (await WalletController.GetLabels(user.Id)).OrderBy(l=>l.Name);
        _categories = (await WalletController.GetCategories(user.Id)).OrderBy(w=>w.Name);
    }

    protected override async Task OnParametersSetAsync()
    {        
        if (string.IsNullOrEmpty(Id))
        {
            return;
        }
        
        NewEntry ??= await WalletController.GetEntry(Id);
        
        _buttonName = "Update";
        _isEditMode = true;
    }

    protected async void Submit()
    { 
        if (_isEditMode)
        {
            NewEntry.EntryId = Id;
            await WalletController.UpdateEntry(NewEntry);
            return;
        }
        
        await WalletController.AddNewEntry(NewEntry);
        NewEntry = new() { Date = DateOnly.FromDateTime(DateTime.Now) };
    }
}
