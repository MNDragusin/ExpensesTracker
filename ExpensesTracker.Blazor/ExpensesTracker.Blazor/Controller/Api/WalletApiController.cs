using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Blazor.Controller.Api;

[ApiController]
[Route("api/Wallet")]
public class WalletApiController(IWalletRepository walletRepository, ILogger<WalletApiController> logger)
    : ControllerBase
{
    private readonly IWalletRepository _walletRepository = walletRepository;
    private readonly ILogger<WalletApiController> _logger = logger;

    [HttpGet("wallets-list")]
    public async Task<ActionResult<WalletsListViewModel>> GetWalletsAsync()
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized("User is not authenticated");
        }

        var userId = User.Claims.FirstOrDefault()!.Value;
        
        WalletsListViewModel walletsList = new WalletsListViewModel();
        walletsList.Wallets = new();
        var wallets = await _walletRepository.GetWallets(userId);
            
        foreach (var wallet in wallets){
            var walletData = await GetWallet(wallet.Id, userId);
            walletsList.Wallets.Add(walletData);
        }

        return Ok(walletsList);
    }
    
    private  async Task<WalletViewModel> GetWallet(string walletId, string userId)
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

        WalletViewModel walletViewModel = new WalletViewModel(){
            WalletId = currentWallet.Id,
            WalletName = currentWallet.Name,
            Entries = entries,
            TotalAmount = totalAmount,
            ColorCode = currentWallet.ColorCode
        };

        return walletViewModel;
    }
}