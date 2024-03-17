using System.Globalization;
using CsvHelper;
using ExpensesTracker.Common.DataContext.Sqlite;
using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Server.Services;

public class DataImporter : IDataImporter
{
    private readonly ExpensesContext _expensesContext;

    public DataImporter(ExpensesContext expensesContext)
    {
        _expensesContext = expensesContext;
    }
    
    public async Task<IEnumerable<WalletEntry>> ParseCSVAsync(IBrowserFile file, string userId)
    {
        List<WalletEntry>? result = new();

        try
        {
            using (var stream = new MemoryStream())
            {
                var culture = CultureInfo.CreateSpecificCulture("de-DE");
                culture.NumberFormat = new NumberFormatInfo() { 
                    CurrencyDecimalSeparator = ".",
                    CurrencyGroupSeparator = ",",
                    };
                    culture.DateTimeFormat.DateSeparator = "/";
                await file.OpenReadStream().CopyToAsync(stream);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                using (CsvReader csvReader = new CsvReader(reader, culture))
                {
                    csvReader.Context.Configuration.Delimiter = ";";
                    csvReader.Context.Configuration.IgnoreBlankLines = true;
                    csvReader.Context.Configuration.HasHeaderRecord = true;
                    csvReader.Context.Configuration.MissingFieldFound = null;
                    csvReader.Context.Configuration.NewLine = Environment.NewLine;

                    csvReader.Context.RegisterClassMap<WalletEntryMap>();

                    while (csvReader.Read())
                    {
                        //result.Add(csvReader.GetRecord<WalletEntry>());
                        await UpdateDb(csvReader.GetRecord<WalletEntry>(), userId);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            string error = $"error: {e.Message}";
            Console.WriteLine(error);
        }
        
        return result;
    }

    private async Task<bool> UpdateDb(WalletEntry entry, string userId)
    {
        Console.WriteLine($"user {userId}");
        Owner? owner = await _expensesContext.Owners.FindAsync(userId);
        
        if (owner == null)
        {
            owner = new Owner()
            {
                OwnerId = userId,
                Name = userId,
                Wallets = new List<Wallet>(),
                Categories = new List<Category>(),
                Labels = new List<Label>()
            };

            await _expensesContext.Owners.AddAsync(owner);
            //await _expensesContext.SaveChangesAsync();
        }

        var wallet =await _expensesContext.Wallets.Where(w =>w.OwnerId == userId && w.Name == entry.WalletId).FirstOrDefaultAsync();
        if (wallet == null)
        {
            wallet = new Wallet()
            {
                OwnerId = userId,
                Name = entry.WalletId
            };

            wallet = (await _expensesContext.Wallets.AddAsync(wallet)).Entity;
        }

        var category = await _expensesContext.Categories.Where(c => c.OwnerId == userId && c.Name == entry.CategoryId).FirstOrDefaultAsync();
        if (category == null)
        {
            category = new Category()
            {
                Name = entry.CategoryId,
                OwnerId = userId
            };

            category = (await _expensesContext.Categories.AddAsync(category)).Entity;
        }

        var label = await _expensesContext.Labels.Where(l => l.OwnerId == userId && l.Name == entry.LabelId).FirstOrDefaultAsync();
        if (label == null)
        {
            label = new Label()
            {
                Name = entry.LabelId,
                OwnerId = userId
            };

            label = (await _expensesContext.Labels.AddAsync(label)).Entity;
        }

        entry.WalletId = wallet.WalletId;
        entry.CategoryId = category.Id;
        entry.LabelId = label.Id;
        var newEntry = new WalletEntry()
        {
            WalletId = entry.WalletId,
            CategoryId = entry.CategoryId,
            LabelId = entry.LabelId,
            Amount = entry.Amount,
            Date = entry.Date
        };
        
        newEntry = (await _expensesContext.WalletEntries.AddAsync(newEntry)).Entity;
        return (await _expensesContext.SaveChangesAsync()) != 0;
    }
}