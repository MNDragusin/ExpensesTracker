using AppDataContext;
using CommunityToolkit.Mvvm.ComponentModel;
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
    private List<WalletEntry> _currentEntries = new();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("name", out var obj)) {
            _ = LoadDataAsync((string)obj);
        }
    }

    private async Task LoadDataAsync(string nameId)
    {
        try
        {
            var currentWallet = await _dbContext.Wallets.FirstOrDefaultAsync(p => p.Name == nameId);

            var categories = await _dbContext.Categories.Where(c => c.OwnerId == currentWallet.OwnerId).ToListAsync();
            var labels = await _dbContext.Labels.Where(l => l.OwnerId == currentWallet.OwnerId).ToListAsync();

            var entries = await _dbContext.WalletEntries.Where(p => p.WalletId == currentWallet.Id).ToListAsync();

            foreach (var entry in entries)
            {
                entry.Category = categories.FirstOrDefault(c => c.Id == entry.CategoryId);
                entry.Label = labels.FirstOrDefault(l => l.Id == entry.LabelId);
            }

            CurrentEntries = entries;
        }
        catch (Exception ex)
        {
            _errorHandler.HandleError(ex);
        }
    }
}