using ExpensesTracker.Common.Dtos;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Client.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletApiController(IWalletRepository walletRepository, ILogger<WalletApiController> logger)
: ControllerBase
{
    [HttpPost("AddNewWallet")]
    public async Task<ActionResult<WalletDto>> AddNewWallet([FromBody] WalletDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Name))
        {
            logger.LogError("Invalid wallet data provided.");
            return BadRequest("Invalid wallet data provided.");
        }
        var entity = dto.MapToEntity();
        var result = await walletRepository.AddNewWalletAsync(entity);
        if (result == null)
        {
            logger.LogError("Failed to add new wallet to Db.");
            return BadRequest("Failed to add new wallet.");
        }
        return Ok(result.MapToDto());
    }

    [HttpPost("AddNewEntry")]
    public async Task<ActionResult<WalletEntryDto>> AddNewEntry([FromBody] WalletEntryDto dto)
    {
        var entity = dto.MapToEntity();

        var result = await walletRepository.AddNewEntryAsync(entity);
        if (result == null)
        {
            logger.LogError("Failed to add new entry to Db.");
            return BadRequest("Invalid data provided.");
        }

        return Ok(result.MapToDto());
    }

    [HttpGet("GetAllEntries/{walletId}")]
    public async Task<ActionResult<List<WalletEntryDto>>> GetAllEntries(string walletId)
    {
        if (string.IsNullOrEmpty(walletId))
        {
            logger.LogError("Wallet ID is null or empty.");
            return BadRequest("Invalid wallet ID provided.");
        }

        var entries = await walletRepository.GetAllEntriesAsync(walletId);
        if (entries == null || !entries.Any())
        {
            logger.LogInformation("No entries found for the provided wallet ID.");
            return NotFound("No entries found.");
        }

        return Ok(entries.Select(x => x.MapToDto()).ToList());
    }
}

