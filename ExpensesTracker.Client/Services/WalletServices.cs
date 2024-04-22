using System;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Shared;
using System.Net.Http.Json;

namespace ExpensesTracker.Client.Services
{
	public class WalletServices : IWalletController
	{
        private readonly HttpClient _httpClient;

		public WalletServices(HttpClient httpClient)
		{
            _httpClient = httpClient;
		}

        public async Task<Category> AddNewCategory(Category newCategory)
        {
            newCategory.Id = string.Empty;
            var result = await _httpClient.PostAsJsonAsync("/api/ExpensesCrud/newCategory", newCategory);
            if(!result.IsSuccessStatusCode){
                return null;
            }

            return await result.Content.ReadFromJsonAsync<Category>();
        }

        public async Task<WalletEntry> AddNewEntry(WalletEntry walletEntry)
        {   
            walletEntry.EntryId = string.Empty;
            var result = await _httpClient.PostAsJsonAsync("/api/ExpensesCrud/newWalletEntry", walletEntry);
            if(!result.IsSuccessStatusCode){
                return null;
            }

            return await result.Content.ReadFromJsonAsync<WalletEntry>();
        }

        public async Task<Label> AddNewLabel(Label newLabel)
        {
            newLabel.Id = string.Empty;
            var result = await _httpClient.PostAsJsonAsync("/api/ExpensesCrud/newLabel", newLabel);
                        if(!result.IsSuccessStatusCode){
                return null;
            }

            return await result.Content.ReadFromJsonAsync<Label>();
        }

        public async Task<Wallet> AddNewWallet(Wallet newWallet)
        {
            newWallet.Id = string.Empty;
            var result = await _httpClient.PostAsJsonAsync("/api/ExpensesCrud/newWallet", newWallet);
            if(!result.IsSuccessStatusCode){
                return null;
            }

            return await result.Content.ReadFromJsonAsync<Wallet>();
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _httpClient.DeleteAsync($"/api/ExpensesCrud/removeEntry?id={id}");
            return result.IsSuccessStatusCode;
        }

        public Task<bool> DeletWallet(string walletId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WalletEntry>> GetAllExpenses(string walletId)
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<WalletEntry>>($"/api/ExpensesCrud/walletEntries?walletId={walletId}");
            return result;
        }

        public async Task<IEnumerable<Category>> GetCategories(string ownerId)
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<Category>>("/api/ExpensesCrud/categories");
            return result;
        }

        public async Task<WalletEntry> GetEntry(string entryId)
        {
            var result = await _httpClient.GetFromJsonAsync<WalletEntry>($"/api/ExpensesCrud/entry?entryId={entryId}");
            return result;
        }

        public async Task<IEnumerable<Label>> GetLabels(string ownerId)
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<Label>>("/api/ExpensesCrud/lables");
            return result;
        }

        public async Task<IEnumerable<Wallet>> GetWallets(string ownerId)
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<Wallet>>("/api/ExpensesCrud/wallets");
            return result;
        }

        public Task<WalletEntry> UpdateEntry(WalletEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
