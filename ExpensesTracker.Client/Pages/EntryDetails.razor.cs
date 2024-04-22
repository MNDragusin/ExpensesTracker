using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Shared;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Client.Pages;

public class EntryDetailsBase : AuthComponentBase
{
    [Parameter] public string Id { get; set; }
    [Inject] IWalletController WalletController { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected IEnumerable<Wallet>? _wallets;
    protected IEnumerable<Label>? _labels;
    protected IEnumerable<Category>? _categories;
    protected WalletEntry? NewEntry { get; set; } = null;
    
    protected string CategoryIdSelected {get;set;} = string.Empty;
    protected string _buttonName = "Add new";
    private bool _isEditMode = false;
    protected DateTime? DateTimeVar;

    protected float AddedValue { get; set; }
    protected bool ShowSuccessAlert { get; set; }
    protected bool ShowLoading = true;
    
     protected override async Task OnInitializedAsync()
    {
        ShowSuccessAlert = false;
        ShowLoading = true;
        var claim = await TryGetAuthenticatedUser();
        if(string.IsNullOrEmpty(claim)){
            return;
        }

        if (!NavigationManager.Uri.Contains("/editEntry/"))
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
            ShowSuccessAlert = false;
            StateHasChanged();
            return;
        }

        ShowSuccessAlert = true;
        AddedValue = result.Amount;
        NewEntry = new() { Date = DateOnly.FromDateTime(DateTime.Now) };
        StateHasChanged();
    }

    protected async void OnNewLabel(string newLabelName)
    {
        var user = await TryGetAuthenticatedUser();
        if(string.IsNullOrEmpty(user)){
            return;
        }

        Label label = new Label() { Name = newLabelName, OwnerId = user };
        var restul = await WalletController.AddNewLabel(label);
        _labels = _labels.Append(restul).OrderBy(l => l.Name);

        StateHasChanged();
    }

    protected async void OnNewWallet(string newWalletName)
    {
        var user = await TryGetAuthenticatedUser();
        if(string.IsNullOrEmpty(user)){
            return;
        }

        Wallet wallet = new() { Name = newWalletName, OwnerId = user };
        var restul = await WalletController.AddNewWallet(wallet);
        _wallets = _wallets.Append(restul).OrderBy(l => l.Name);

        StateHasChanged();
    }

    protected async void OnNewCategory(string newCategoryName)
    {
        var user = await TryGetAuthenticatedUser();
        if(string.IsNullOrEmpty(user)){
            return;
        }
        Category category = new() { Name = newCategoryName, OwnerId = user };
        var restul = await WalletController.AddNewCategory(category);
        _categories = _categories.Append(restul).OrderBy(l => l.Name);

        StateHasChanged();
    }
}
