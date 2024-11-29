using System.Text.Json.Serialization;

namespace ExpensesTracker.Models;

public class WalletsListViewModel
{
    public WalletsListViewModel(bool hasData = true)
    {
        HasData = hasData;
    }
    
    [JsonConstructor]
    public WalletsListViewModel()
    {
        
    }
    
    public readonly bool HasData;
    public List<WalletViewModel> Wallets { get; set; }
}
