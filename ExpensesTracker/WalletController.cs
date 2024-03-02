using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Server.Controllers;
public class WalletController : IWalletController
{
    [HttpGet]
    public async Task<IEnumerable<ExpensesEntry>> GetAllExpenses()
    {
        //mock
        await Task.Delay(2000);
        return GenerateMockData(50);
    }

    private IEnumerable<ExpensesEntry> GenerateMockData(int count)
    {
        List<ExpensesEntry> mockData = new List<ExpensesEntry>();
        for (int i = 0; i < count; i++)
        {
            mockData.Add(new ExpensesEntry()
            {
                Id = i,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                WalletId = "mdWallet",
                Category = "food",
                Label = "Rewe",
                Amount = i + 10
            });
        }

        return mockData;
    }
}

public interface IWalletController
{
    public Task<IEnumerable<ExpensesEntry>> GetAllExpenses();
}