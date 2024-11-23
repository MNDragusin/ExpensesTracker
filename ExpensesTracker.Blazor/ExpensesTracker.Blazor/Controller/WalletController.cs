using ExpensesTracker.Blazor.Client;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    public class WalletController : ControllerBase, IWalletController
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IWalletServices _walletServices;
        private AuthenticationStateProvider _authenticationStateProvider;
        
        public WalletController(ILogger<WalletController> logger, IWalletServices walletServices, AuthenticationStateProvider authenticationStateProvider)
        {
            _logger = logger;
            _walletServices = walletServices;
            _authenticationStateProvider = authenticationStateProvider;
        }

        // GET: WalletController
        [Route("[controller]/[action]/{walletId}")]
        public async Task<ActionResult<WalletViewModel>> Expenses(string? walletId)
        {
            if (string.IsNullOrEmpty(walletId))
            {
                return BadRequest();
            }
            
            var walletViewModel = await GetWallet(walletId);    
            return Ok(walletViewModel);
        }

        private  async Task<WalletViewModel> GetWallet(string walletId){
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            string id = string.Empty;
            
            if (user.Identity is { IsAuthenticated: true })
            {
                // Extract the username from the claims
                id = user.Claims.FirstOrDefault().Value; // Default claim for username
            }
            
            var wallets = await _walletServices.GetWallets(id);

            var currentWallet = wallets.Where(e => e.Id == walletId).FirstOrDefault();
            currentWallet.Entries = await _walletServices.GetAllExpenses(currentWallet.Id);
            var labels = await _walletServices.GetLabels(id);
            var categories = await _walletServices.GetCategories(id);

            float totalAmount = 0f;
            List<Entry> entries = new List<Entry>();
            foreach (var entry in currentWallet.Entries){
                totalAmount += entry.Amount;
                entries.Add(new Entry(){
                    EntryId = entry.EntryId,
                    Date = entry.Date,
                    Amount = entry.Amount,
                    Label = labels.FirstOrDefault(e=> e.Id == entry.LabelId).Name,
                    Category = categories.FirstOrDefault(e=>e.Id == entry.CategoryId).Name
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

        [Route("[controller]/[action]")]
        public async Task<WalletsListViewModel> Wallets()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            string id = string.Empty;
            
            if (user.Identity is { IsAuthenticated: true })
            {
                // Extract the username from the claims
                id = user.Claims.FirstOrDefault().Value; // Default claim for username
            }
            
            WalletsListViewModel walletsList = new WalletsListViewModel();
            walletsList.Wallets = new();
            var wallets = await _walletServices.GetWallets(id);
            
            foreach (var wallet in wallets){
                var walletData = await GetWallet(wallet.Id);
                walletsList.Wallets.Add(walletData);
            }

            return walletsList;
        }

        [Route("[controller]/newEntry")]
        [Route("[controller]/[action]/{entryId}")]
        public async Task<ActionResult<WalletEntry>> Edit(string? entryId)
        {
            var entry = new WalletEntry(){
                WalletId = string.Empty,
                EntryId = string.Empty,
                CategoryId = string.Empty,
                LabelId = string.Empty
            };

            var ownerId = User.Claims.First().Value;
            var categories = await _walletServices.GetCategories(ownerId);
            var selectedCatList = new SelectList(categories.Select(c=> new {Text = c.Name, Value = c.Id}), "Value", "Text");
            //ViewBag.Categories = selectedCatList;

            var labels = await _walletServices.GetLabels(ownerId);
            var selectedLblList = new SelectList(labels.Select(c=> new {Text = c.Name, Value = c.Id}), "Value", "Text");
            //ViewBag.Labels = selectedLblList;

            if(!string.IsNullOrEmpty(entryId)){
                entry = await _walletServices.GetEntry(entryId);
            }
            
            return Ok(entry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<WalletEntry>> SaveEntry(WalletEntry entry){
            if(ModelState.IsValid){
                return RedirectToAction("Expenses");
            }

            return Ok(entry);
        }
    }
}
