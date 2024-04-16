using Microsoft.AspNetCore.Mvc;
using ExpensesTracker.Shared;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.AspNetCore.Authorization;


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
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var wallets = await _walletControler.GetWallets(userId);
            if (wallets == null || wallets.Count() == 0)
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
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var categories = await _walletControler.GetCategories(userId);
            if (categories == null || categories.Count() == 0)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("lables")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WalletEntry>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetLabels()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var lables = await _walletControler.GetLabels(userId);
            if (lables == null || lables.Count() == 0)
            {
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
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var wallets = await _walletControler.GetWallets(userId);
            if (wallets == null || wallets.Count() == 0)
            {
                return NotFound();
            }

            wallets = wallets.Where(w => w.OwnerId == userId);
            if (wallets == null || wallets.Count() == 0)
            {
                return Unauthorized();
            }

            var entries = await _walletControler.GetAllExpenses(walletId);
            if (entries == null || entries.Count() == 0)
            {
                return NotFound();
            }

            return Ok(entries);
        }

        [HttpGet("entry")]
        [ProducesResponseType(200, Type = typeof(WalletEntry))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEntryById(string entryId){

            if(string.IsNullOrEmpty(entryId)){
                return BadRequest();
            }

            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var result = await _walletControler.GetEntry(entryId);

            if(result == null){
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost("newWalletEntry")]
        [ProducesResponseType(200, Type = typeof(WalletEntry))]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddNewEntry([FromBody] WalletEntry walletEntry)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            var result = await _walletControler.AddNewEntry(walletEntry);

            return CreatedAtRoute(
                routeValues: new { id = result.EntryId },
                value: result
            );
        }

        [HttpPost("newWallet")]
        [ProducesResponseType(200, Type = typeof(Wallet))]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddNewWallet([FromBody] Wallet newWallet){
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            var result = await _walletControler.AddNewWallet(newWallet);

            return CreatedAtRoute(
                routeValues: new { id = result.Id },
                value: result
            );
        }

        [HttpPost("newCategory")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddNewCategory([FromBody] Category newCategory){
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            var result = await _walletControler.AddNewCategory(newCategory);

            return CreatedAtRoute(
                routeValues: new { id = result.Id },
                value: result
            );
        }

        [HttpPost("newLabel")]
        [ProducesResponseType(200, Type = typeof(Label))]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddNewLabel([FromBody] Label newLabel){
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            var result = await _walletControler.AddNewLabel(newLabel);

            return CreatedAtRoute(
                routeValues: new { id = result.Id },
                value: result
            );
        }

        [HttpDelete("removeEntry")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEntry(string id){
             if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

            var entry = await _walletControler.GetEntry(id);
            if(entry is null || entry.EntryId != id){
                return NotFound();
            }

            var result = await _walletControler.Delete(id);
            if(!result){
                return BadRequest("");
            }

            return Ok();
        }
        
    }
}

