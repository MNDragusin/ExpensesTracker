using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Server.Services;

public class WalletController : IWalletController
{
    [HttpGet]
    public async Task<IEnumerable<WalletEntry>> GetAllExpenses()
    {
        //mock
        await Task.Delay(2000);
        return GenerateMockData(50);
    }

    private IEnumerable<WalletEntry> GenerateMockData(int count)
    {
        List<WalletEntry> mockData = new List<WalletEntry>();
        for (int i = 0; i < count; i++)
        {
            mockData.Add(new WalletEntry()
            {
                EntryId= i,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                WalletId = "mdWallet",
                CategoryId = "food",
                LabelId = "Rewe",
                Amount = i + 10
            });
        }

        return mockData;
    }
}