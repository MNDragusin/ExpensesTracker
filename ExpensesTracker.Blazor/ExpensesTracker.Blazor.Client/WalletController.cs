using System.Net.Http.Json;
using ExpensesTracker.Models;

namespace ExpensesTracker.Blazor.Client.Services;

public class WalletController : IWalletController
{
    private readonly HttpClient _httpClient;

    public WalletController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<WalletsListViewModel> Wallets()
    {
       var result = await _httpClient.GetFromJsonAsync<WalletsListViewModel>("api/Wallet/wallets-list");
       return result;
    }
}