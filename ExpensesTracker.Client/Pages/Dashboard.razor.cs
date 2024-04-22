using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ExpensesTracker.Client.Pages;

public class DashboardBase : ComponentBase
{
    [Inject] AuthenticationStateProvider? _persistentAuthenticationStateProvider { get; set; }
    [Inject] IWalletController? _walletController { get; set; }

    protected double TotalAmount { get; set; }
    protected bool ShowLoading = true;
    protected IEnumerable<Wallet>? _wallets;
    protected IEnumerable<Category>? Categories;
    protected IEnumerable<Label>? Labels;

    //protected List<ChartSeries> Series = new();
    protected readonly string[] XAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    protected List<int> Years = new();
    private int _selectedYear;
    protected int SelectedYear
    {
        get { return _selectedYear; }
        set
        {
            if (_selectedYear == value)
            {
                return;
            }

            _selectedYear = value;
            ApplyFilters();
        }
    }

    private string _currentSelectedCategory;
    protected string CurrentSelectedCategory
    {
        get { return _currentSelectedCategory; }
        set
        {
            if (_currentSelectedCategory == value)
            {
                return;
            }

            _currentSelectedCategory = value;
            ApplyFilters();
        }
    }

    private string _currentSelectedLabel;
    protected string CurrentSelectedLabel
    {
        get { return _currentSelectedLabel; }
        set
        {
            if (_currentSelectedLabel == value)
            {
                return;
            }

            _currentSelectedLabel = value;
            ApplyFilters();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        ShowLoading = true;
        if (!await TryFetchData())
        {
            return;
        }

        SelectedYear = DateTime.Now.Year;

        await Task.Delay(500);
        ShowLoading = false;
    }

    private async Task<bool> TryFetchData()
    {
        AuthenticationState state = await _persistentAuthenticationStateProvider!.GetAuthenticationStateAsync();

        if (!state.User.Identity.IsAuthenticated)
        {
            return false;
        }

        string claim = string.Empty;
        claim = state.User.Claims.FirstOrDefault().Value;

        _wallets = await _walletController.GetWallets(claim);

        Categories = await _walletController.GetCategories(claim);
        Categories = Categories.OrderBy(c=>c.Name);
        Labels = await _walletController.GetLabels(claim);
        Labels = Labels.OrderBy(l=>l.Name);

        foreach (var wallet in _wallets)
        {
            if (wallet.Entries is null)
            {
                wallet.Entries = await _walletController.GetAllExpenses(wallet.Id);
                foreach (var entry in wallet.Entries)
                {
                    if (!Years.Contains(entry.Date.Year))
                    {
                        Years.Add(entry.Date.Year);
                    }
                }
            }
        }

        return true;
    }

    private void ApplyFilters()
    {
        TotalAmount = 0;
        //Series.Clear();

        foreach (var wallet in _wallets!)
        {
            //ChartSeries chartData = new ChartSeries();
            //chartData.Data = new double[12];

            wallet.Entries.OrderBy(e => e.Date);
            wallet.TotalAmount = 0;

            var entries = wallet.Entries.Where(e => e.Date.Year == SelectedYear).Where(e =>
            {
                if (string.IsNullOrEmpty(CurrentSelectedCategory))
                {
                    return true;
                }

                return e.CategoryId == CurrentSelectedCategory;
            }).Where(e =>
            {
                if (string.IsNullOrEmpty(CurrentSelectedLabel))
                {
                    return true;
                }

                return e.LabelId == CurrentSelectedLabel;
            });

            foreach (var entry in entries)
            {
                wallet.TotalAmount += entry.Amount;
                //chartData.Data[entry.Date.Month - 1] += entry.Amount;
            }

            TotalAmount += wallet.TotalAmount;
            //chartData.Name = $"{wallet.Name}: {Math.Round(Convert.ToDecimal(wallet.TotalAmount), 2)} \u20AC";

            Years = Years.OrderDescending().ToList();

            //Series.Add(chartData);
        }
    }
}
