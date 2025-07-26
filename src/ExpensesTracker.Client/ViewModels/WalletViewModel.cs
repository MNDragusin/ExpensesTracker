using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using ExpensesTracker.Common.Dtos;
using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Client.ViewModels;

public class WalletViewModel : INotifyPropertyChanged
{
    private NavigationManager _navigationManager;
    private HttpClient _httpClient;
    private ILogger<WalletViewModel> _logger;
    private const string Endpoint = "api/wallet/";
    
    public WalletDto WalletData { get; set; }
    public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    public List<LabelDto> Labels { get; set; } = new List<LabelDto>();
    
    public WalletViewModel(HttpClient httpClient, NavigationManager navigationManager, ILogger<WalletViewModel> logger)
    {
        _httpClient = httpClient;
        _navigationManager =  navigationManager;
        _logger = logger;
        
        WalletData = new WalletDto()
        {
            Name = "##Template##",
            Entries = new List<EntryDto>(),
        };
        
        Init();
    }

    private async Task GetWallets()
    {
        var result = await _httpClient.GetAsync($"{Endpoint}getAllWallets");
        if (result.IsSuccessStatusCode)
        {
            var wallets = await result.Content.ReadFromJsonAsync<List<WalletDto>>();
            if (wallets != null && wallets.Count > 0)
            {
                WalletData = wallets[0];
            }
        }
        else
        {
            _logger.LogError($"Error fetching wallets: {result.ReasonPhrase}");
            Console.WriteLine($"Error fetching wallets: {result.ReasonPhrase}");
        }
    }

    private async Task Init()
    {
        try
        {
            await GetWallets();
            await GetCategories();
            await GetLabels();

            await GetEntries();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Initialization error: {ex.Message}");
            Console.WriteLine($"Initialization error: {ex.Message}");
        }
        
        OnPropertyChanged();
    }

    private async Task GetEntries()
    {
        var result = await _httpClient.GetAsync($"{Endpoint}getAllEntries/{WalletData.Id}");
        if (result.IsSuccessStatusCode)
        {
            var entries = await result.Content.ReadFromJsonAsync<List<EntryDto>>();
            if (entries != null)
            {
                WalletData.Entries = entries;
                WalletData.TotalAmount = entries.Sum(e => e.Amount);
            }
        }
        else
        {
            _logger.LogError($"Error fetching wallet entries: {result.ReasonPhrase}");
            Console.WriteLine($"Error fetching wallet entries: {result.ReasonPhrase}");
        }
    }

    private async Task GetCategories()
    {
        var result = await _httpClient.GetAsync($"{Endpoint}getCategories");
        if (result.IsSuccessStatusCode)
        {
            Categories = await result.Content.ReadFromJsonAsync<List<CategoryDto>>();
        }
        else
        {
            _logger.LogError($"Error fetching categories: {result.ReasonPhrase}");
            Console.WriteLine($"Error fetching categories: {result.ReasonPhrase}");
        }
    }

    private async Task GetLabels() //TODO maybe change the name to Tags
    {
        var result = await _httpClient.GetAsync($"{Endpoint}getLabels");
        if (result.IsSuccessStatusCode)
        {
            Labels = await result.Content.ReadFromJsonAsync<List<LabelDto>>();
        }
        else
        {
            _logger.LogError($"Error fetching labels: {result.ReasonPhrase}");
            Console.WriteLine($"Error fetching labels: {result.ReasonPhrase}");
        }
    }

    public async Task CreateWallet(BaseDto baseDto)
    {
        try
        {
            var wallet = new WalletDto
            {
                Id = baseDto.Id,
                Name = baseDto.Name,
                ColorCode = baseDto.ColorCode
            };
            
            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}addNewWallet", wallet);

            if (response.IsSuccessStatusCode)
            {
                WalletData = await response.Content.ReadFromJsonAsync<WalletDto>();
            }
            else
            {
                Console.WriteLine($"Error creating wallet: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        
        OnPropertyChanged();
    }

    public Task DeleteEntry(string id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateCategory(BaseDto dto)
    {
        var result = await _httpClient.PostAsJsonAsync($"{Endpoint}createCategory", dto);
        if (result.IsSuccessStatusCode)
        {
            var category = await result.Content.ReadFromJsonAsync<CategoryDto>();
            if (category != null)
            {
                Categories.Add(category);
            }
        }
        else
        {
            _logger.LogError($"Error creating category: {result.ReasonPhrase}");
            Console.WriteLine($"Error creating category: {result.ReasonPhrase}");
        }
        
        OnPropertyChanged();
    }

    public async Task  CreateLabel(BaseDto dto)
    {
        var result = await _httpClient.PostAsJsonAsync($"{Endpoint}createLabel", dto);
        if (result.IsSuccessStatusCode)
        {
            var label = await result.Content.ReadFromJsonAsync<LabelDto>();
            if (label != null)
            {
                Labels.Add(label);
            }
        }
        else
        {
            _logger.LogError($"Error creating label: {result.ReasonPhrase}");
            Console.WriteLine($"Error creating label: {result.ReasonPhrase}");
        }
        
        OnPropertyChanged();
    }

    public async Task CreateEntry(EntryDto entry)
    {
        entry.WalletId = WalletData.Id;
        var result = await _httpClient.PostAsJsonAsync($"{Endpoint}addNewEntry", entry);
        if (result.IsSuccessStatusCode)
        {
            var newEntry = await result.Content.ReadFromJsonAsync<EntryDto>();
            if (newEntry != null)
            {
                WalletData.Entries.Append(newEntry);
                WalletData.TotalAmount += newEntry.Amount;
            }
        }
        else
        {
            _logger.LogError($"Error creating entry: {result.ReasonPhrase}");
            Console.WriteLine($"Error creating entry: {result.ReasonPhrase}");
        }
        
        OnPropertyChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}