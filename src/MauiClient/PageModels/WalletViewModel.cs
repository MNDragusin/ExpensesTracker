using System.Collections.ObjectModel;
using AppDataContext;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace MauiClient.PageModels;

public partial class WalletViewModel : ObservableObject, IQueryAttributable
{
    public WalletViewModel(DataContext dataContext, ModalErrorHandler errorHandler)
    {
        _dbContext = dataContext;
        _errorHandler = errorHandler;
    }
    
    private readonly DataContext _dbContext;
    private readonly ModalErrorHandler _errorHandler;

    [ObservableProperty]
    private ObservableCollection<WalletEntry> _currentEntries = new();

    [ObservableProperty]
    private List<float> _categorySums = new();
    [ObservableProperty]
    private List<Brush> _categoryBrushes = new();

    [ObservableProperty]
    private bool _isBusy = true;

    [ObservableProperty]
    private WalletEntry _selectedEntry;

    [ObservableProperty] private WalletsPage.MyCanvas _mainChart;
    
    private string _currentWalletId;

    public Action InvalidateRequest;
    
    [RelayCommand]
    public void OnEntrySelected()
    {
        if (SelectedEntry is null)
        {
            return;
        }

        Shell.Current.GoToAsync($"task", new ShellNavigationQueryParameters(){
                    {EntryDetailPageModel.EntryQueryKey, SelectedEntry}
                });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("refresh") && !string.IsNullOrEmpty(_currentWalletId))
        {
            CurrentEntries.Remove(SelectedEntry);
            return;
        }

        if (!query.TryGetValue("name", out var obj)) return;
        
        _currentWalletId = (string)obj;
        MainChart = new WalletsPage.MyCanvas(0, 0, 220, 220);
        _ = LoadDataAsync(_currentWalletId);
    }

    private async Task LoadDataAsync(string nameId)
    {
        try
        {
            var currentWallet = await _dbContext.Wallets.FirstOrDefaultAsync(p => p.Name == nameId);

            var categories = await _dbContext.Categories.Where(c => c.OwnerId == currentWallet!.OwnerId).ToListAsync();

            foreach (var category in categories)
            {
                CategoryBrushes.Add(new SolidColorBrush(Color.FromArgb(category.ColorCode)));
            }

            var labels = await _dbContext.Labels.Where(l => l.OwnerId == currentWallet!.OwnerId).ToListAsync();

            var entries = await _dbContext.WalletEntries.Where(p => p.WalletId == currentWallet!.Id).ToListAsync();

            float expensesAmount = 0;
            foreach (var entry in entries)
            {
                entry.Category = categories.FirstOrDefault(c => c.Id == entry.CategoryId);
                entry.Label = labels.FirstOrDefault(l => l.Id == entry.LabelId);
                expensesAmount += entry.Amount;
            }
            
            MainChart.UpdateValues(expensesAmount, expensesAmount/3);
            InvalidateRequest?.Invoke();
            
            CurrentEntries = new(entries);
            
            foreach (var cat in categories)
            {
                CategorySums.Add(CurrentEntries.Where(c => c.CategoryId == cat.Id).Sum(e => e.Amount));
            }
        }
        catch (Exception ex)
        {
            _errorHandler.HandleError(ex);
        }

        IsBusy = false;
    }
}