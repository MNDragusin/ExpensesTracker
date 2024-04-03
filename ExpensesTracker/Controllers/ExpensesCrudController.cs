using Microsoft.AspNetCore.Mvc;
using ExpensesTracker.Shared;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesCrudController : ControllerBase
    {
        private readonly IWalletController _walletControler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExpensesCrudController(IWalletController walletController, IHttpContextAccessor httpContextAccessor)
        {
            _walletControler = walletController;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("wallets")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Wallet>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)] //Unauthorized
        public async Task<IActionResult> GetWallets()
        {
            if(!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated){
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var wallets = await _walletControler.GetWallets(userId);
            if(wallets == null || wallets.Count() == 0)
            {
                return NotFound();
            }

            return Ok(wallets);
        }

        [HttpGet("categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)] 
        public async Task<IActionResult> GetCategories()
        {
            if(!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated){
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var categories = await _walletControler.GetCategories(userId);
            if(categories == null || categories.Count() == 0){
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("lables")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WalletEntry>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetLabels(){
            if(!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated){
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var lables = await _walletControler.GetLabels(userId);
            if(lables == null || lables.Count() == 0){
                return NotFound();
            }

            return Ok(lables);
        }

        [HttpGet("walletEntries")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WalletEntry>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEntriesForWallet(string walletId)
        {
            if(!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated){
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var wallets = await _walletControler.GetWallets(userId);
            if(wallets == null || wallets.Count() == 0)
            {
                return NotFound();
            }

            wallets = wallets.Where(w=>w.OwnerId == userId);
            if(wallets == null || wallets.Count() == 0){
                return Unauthorized();
            }

            var entries = await _walletControler.GetAllExpenses(walletId);
            if(entries == null || entries.Count() == 0){
                return NotFound();
            }

            return Ok(entries);
        }
    }
}

