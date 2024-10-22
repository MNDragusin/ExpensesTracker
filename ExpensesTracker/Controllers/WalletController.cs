using ExpensesTracker.Common.EntityModel.Sqlite;
using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWalletServices _walletServices;
        public WalletController(ILogger<HomeController> logger, IWalletServices walletServices)
        {
            _logger = logger;
            _walletServices = walletServices;
        }

        // GET: WalletController
        [Route("[controller]/[action]")]
        public async Task<ActionResult> Expenses()
        {
            var id = User.Claims.First().Value;
            var wallets = await _walletServices.GetWallets(id);
            
            var currentWallet = wallets.First();
            currentWallet.Entries = await _walletServices.GetAllExpenses(currentWallet.Id);
            var labels = await _walletServices.GetLabels(id);
            var categories = await _walletServices.GetCategories(id);

            List<Entry> entries = new List<Entry>();
            foreach (var entry in currentWallet.Entries){
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
                Entries = entries
            };

            return View(walletViewModel);
        }

        [Route("[controller]/[action]/{entryId}")]
        public async Task<ActionResult> Edit(string? entryId){

            var entry = new WalletEntry(){
                WalletId = string.Empty,
                EntryId = string.Empty,
                CategoryId = string.Empty,
                LabelId = string.Empty
            };

            var ownerId = User.Claims.First().Value;
            var categories = await _walletServices.GetCategories(ownerId);
            var selectedCatList = new SelectList(categories.Select(c=> new {Text = c.Name, Value = c.Id}), "Value", "Text");
            ViewBag.Categories = selectedCatList;

            var labels = await _walletServices.GetLabels(ownerId);
            var selectedLblList = new SelectList(labels.Select(c=> new {Text = c.Name, Value = c.Id}), "Value", "Text");
            ViewBag.Labels = selectedLblList;

            if(!string.IsNullOrEmpty(entryId)){
                entry = await _walletServices.GetEntry(entryId);
            }
            
            return View(entry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEntry(WalletEntry entry){
            if(ModelState.IsValid){
                return RedirectToAction("Expenses");
            }

            return View();
        }
    }
}
