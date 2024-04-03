using System;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Shared;
using System.Net.Http.Json;

namespace ExpensesTracker.Client.Services
{
	public class WalletController : IWalletController
	{
        private readonly HttpClient _httpClient;

		public WalletController(HttpClient httpClient)
		{
            _httpClient = httpClient;
		}

        public Task<Category> AddNewCategory(Category newCategory)
        {
            throw new NotImplementedException();
        }

        public Task<WalletEntry> AddNewEntry(WalletEntry walletEntry)
        {
            throw new NotImplementedException();
        }

        public Task<Label> AddNewLabel(Label newLabel)
        {
            throw new NotImplementedException();
        }

        public Task<Wallet> AddNewWallet(Wallet newWallet)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
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

        public Task<WalletEntry> GetEntry(string entryId)
        {
            throw new NotImplementedException();
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

