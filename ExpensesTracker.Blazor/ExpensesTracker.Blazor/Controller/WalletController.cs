using ExpensesTracker.Models;
using ExpensesTracker.Services;

namespace ExpensesTracker.Blazor.Controller;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    
    public WalletService(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }
    
    public async Task<WalletsListViewModel> GetWalletsWithData(string id)
    {
        WalletsListViewModel walletsList = new WalletsListViewModel(true);
        walletsList.Wallets = new();
        var wallets = await _walletRepository.GetWallets(id);
            
        foreach (var wallet in wallets){
            var walletData = await GetWallet(wallet.Id, id);
            walletsList.Wallets.Add(walletData);
        }
        
        return walletsList;
    }
    
    public async Task<WalletViewModel> GetWallet(string walletId, string userId)
    {
        var wallets = await _walletRepository.GetWallets(userId);

        var currentWallet = wallets.Where(e => e.Id == walletId).FirstOrDefault();
        currentWallet.Entries = await _walletRepository.GetAllExpenses(currentWallet.Id);
        var labels = await _walletRepository.GetLabels(userId);
        var categories = await _walletRepository.GetCategories(userId);

        float totalAmount = 0f;
        List<Entry> entries = new List<Entry>();
        foreach (var entry in currentWallet.Entries){
            totalAmount += entry.Amount;
            var label = labels.FirstOrDefault(e => e.Id == entry.LabelId);
            var category = categories.FirstOrDefault(e => e.Id == entry.CategoryId);

            entries.Add(new Entry(){
                EntryId = entry.EntryId,
                Date = entry.Date,
                Amount = entry.Amount,
                Label = new BaseModel()
                {
                    Id = label!.Id,
                    Name = label.Name,
                    ColorCode = label.ColorCode,
                },
                Category =  new BaseModel()
                {
                    Id = category!.Id,
                    Name = category.Name,
                    ColorCode = category.ColorCode,
                },
            });
        }

        WalletViewModel walletViewModel = new WalletViewModel(true){
            WalletId = currentWallet.Id,
            WalletName = currentWallet.Name,
            Entries = entries,
            TotalAmount = totalAmount,
            ColorCode = currentWallet.ColorCode
        };

        return walletViewModel;
    }
}

public interface IWalletService
{
    public Task<WalletsListViewModel> GetWalletsWithData(string id);
    public Task<WalletViewModel> GetWallet(string walletId, string userId);
}