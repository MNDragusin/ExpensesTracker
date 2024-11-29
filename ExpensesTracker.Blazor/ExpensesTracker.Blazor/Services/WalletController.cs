using ExpensesTracker.Blazor.Client;
using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpensesTracker.Blazor.Controller
{
    [Authorize]
    public class WalletController : IWalletController
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IWalletRepository _walletRepository;
        private AuthenticationStateProvider _authenticationStateProvider;
        private readonly IWalletService _walletService;
        
        public WalletController(ILogger<WalletController> logger, IWalletRepository walletRepository, AuthenticationStateProvider authenticationStateProvider, IWalletService walletService)
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _authenticationStateProvider = authenticationStateProvider;
            _walletService = walletService;
        }

        private async Task<string> GetAuthenticatedUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            
            if (user.Identity is { IsAuthenticated: false })
            {
                throw new UnauthorizedAccessException();
            }
            
            return user.Claims.FirstOrDefault()!.Value;
        }

        // GET: WalletController
        [Route("[controller]/[action]/{walletId}")]
        public async Task<WalletViewModel> Expenses(string? walletId)
        {
            if (string.IsNullOrEmpty(walletId))
            {
                return new WalletViewModel(false);
            }
            return await _walletService.GetWallet(walletId, await GetAuthenticatedUser());
        }

        
        
        public async Task<WalletsListViewModel> Wallets()
        {
            return await _walletService.GetWalletsWithData(await GetAuthenticatedUser());
        }

        [Route("[controller]/newEntry")]
        [Route("[controller]/[action]/{entryId}")]
        public async Task<ActionResult<WalletEntry>> Edit(string? entryId)
        {          
            var userId = await GetAuthenticatedUser();
            var entry = new WalletEntry(){
                WalletId = string.Empty,
                EntryId = string.Empty,
                CategoryId = string.Empty,
                LabelId = string.Empty
            };
            
            var categories = await _walletRepository.GetCategories(userId);
            var selectedCatList = new SelectList(categories.Select(c=> new {Text = c.Name, Value = c.Id}), "Value", "Text");
            //ViewBag.Categories = selectedCatList;

            var labels = await _walletRepository.GetLabels(userId);
            var selectedLblList = new SelectList(labels.Select(c=> new {Text = c.Name, Value = c.Id}), "Value", "Text");
            //ViewBag.Labels = selectedLblList;

            if(!string.IsNullOrEmpty(entryId)){
                entry = await _walletRepository.GetEntry(entryId);
            }
            
            return entry;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult<WalletEntry>> SaveEntry(WalletEntry entry){ }
    }
}
