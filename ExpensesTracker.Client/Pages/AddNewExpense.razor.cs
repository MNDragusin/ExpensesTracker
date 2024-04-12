using Microsoft.AspNetCore.Components;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace ExpensesTracker.Pages;

public class AddNewExpenseBase : ComponentBase
{
    [Inject] AuthenticationStateProvider? _persistentAuthenticationStateProvider { get; set; }
    [Inject] IWalletController WalletController { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    [SupplyParameterFromForm] protected WalletEntry? NewEntry { get; set; } = null;
    
    protected IEnumerable<Wallet>? _wallets;
    protected IEnumerable<Label>? _labels;
    protected IEnumerable<Category>? _categories;

    protected string CategoryIdSelected {get;set;} = string.Empty;
    protected string _buttonName = "Add new";
    private bool _isEditMode = false;
    protected DateTime? DateTimeVar;

    protected bool ShowLoading = true;
    [Parameter] public string Id { get; set; }
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

        if (!NavigationManager.Uri.Contains("/EditEntry/"))
        {
            DateTimeVar = DateTime.Now;
            NewEntry ??= new() { Date = DateOnly.FromDateTime((DateTime)DateTimeVar) };
        }
        
        _wallets = (await WalletController.GetWallets(claim)).OrderBy(w=>w.Name);
        _labels = (await WalletController.GetLabels(claim)).OrderBy(l=>l.Name);
        _categories = (await WalletController.GetCategories(claim)).OrderBy(w=>w.Name);
        ShowLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {        
        if (string.IsNullOrEmpty(Id))
        {
            return;
        }
        
        NewEntry ??= await WalletController.GetEntry(Id);
        DateTimeVar = new DateTime(NewEntry.Date, TimeOnly.MinValue);
        _buttonName = "Update";
        _isEditMode = true;
    }

    protected async void Submit()
    {
        NewEntry.Date = DateOnly.FromDateTime((DateTime)DateTimeVar);

        if (_isEditMode)
        {
            NewEntry.EntryId = Id;
            await WalletController.UpdateEntry(NewEntry);
            return;
        }
        
        var result = await WalletController.AddNewEntry(NewEntry);

        if(result == null){
            //do something for the error
        }
        NewEntry = new() { Date = DateOnly.FromDateTime(DateTime.Now) };
    }
}
