using ExpensesTracker.Common.Dtos;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/wallet")]
public class WalletApiController(IWalletRepository walletRepository, ILogger<WalletApiController> logger)
: ControllerBase
{
    [HttpPost("addNewWallet")]
    public async Task<ActionResult<WalletDto>> AddNewWallet([FromBody] WalletDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Name))
        {
            logger.LogError("Invalid wallet data provided.");
            Console.WriteLine("Invalid wallet data provided.");
            return BadRequest("Invalid wallet data provided.");
        }
        
        var entity = dto.MapToWallet(User.Claims.First().Value);
        var result = await walletRepository.AddNewWalletAsync(entity);
        if (result == null)
        {
            logger.LogError("Failed to add new wallet to Db.");
            return BadRequest("Failed to add new wallet.");
        }
        return Ok(result.MapToWalletDto());
    }

    [HttpPost("addNewEntry")]
    public async Task<ActionResult<EntryDto>> AddNewEntry([FromBody] EntryDto dto)
    {
        var entity = dto.MapToEntry();

        var result = await walletRepository.AddNewEntryAsync(entity);
        if (result == null)
        {
            logger.LogError("Failed to add new entry to Db.");
            return BadRequest("Invalid data provided.");
        }

        return Ok(result.MapToEntryDto());
    }
    
    [HttpGet("getAllWallets")]
    public async Task<ActionResult<List<WalletDto>>> GetAllWallets()
    {
        var result = await walletRepository.GetWalletsAsync(User.Claims.First().Value);
        return Ok(result.Select(x => x.MapToWalletDto()).ToList());
    }

    [HttpGet("getAllEntries/{walletId}")]
    public async Task<ActionResult<List<EntryDto>>> GetAllEntries(string walletId)
    {
        if (string.IsNullOrEmpty(walletId))
        {
            logger.LogError("Wallet ID is null or empty.");
            return BadRequest("Invalid wallet ID provided.");
        }

        var entries = await walletRepository.GetAllEntriesAsync(walletId);
        if (entries == null)
        {
            logger.LogInformation("No entries found for the provided wallet ID.");
            return NotFound("No entries found.");
        }

        return Ok(entries.Select(x => x.MapToEntryDto()).ToList());
    }
    
    [HttpGet("getCategories")]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var result = await walletRepository.GetCategoriesAsync(User.Claims.First().Value);
        return Ok(result.Select(x => x.MapToCategoryDto()).ToList());
    }
    
    [HttpGet("getLabels")]
    public async Task<ActionResult<List<LabelDto>>> GetLabels()
    {
        var result = await walletRepository.GetLabelsAsync(User.Claims.First().Value);
        return Ok(result.Select(l => l.MapToLabelDto()).ToList());
    }

    [HttpPost("createCategory")]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto dto)
    {
        var cat = dto.MapToCategory(User.Claims.First().Value);
        var result = await walletRepository.AddNewCategoryAsync(cat);
        return Ok(result.MapToCategoryDto());
    }

    [HttpPost("createLabel")]
    public async Task<ActionResult<LabelDto>> CreateLabel([FromBody] LabelDto dto)
    {
        var label = dto.MapToLabel(User.Claims.First().Value);
        var result = await walletRepository.AddNewLabelAsync(label);
        return Ok(result.MapToLabelDto());
    }
}

