using CsvHelper.Configuration;
using ExpensesTracker.Common.EntityModel.Sqlite;

namespace ExpensesTracker;

public sealed class WalletEntryMap : ClassMap<WalletEntry>
{
    public WalletEntryMap()
    {
        Map(m => m.EntryId).Ignore();
        Map(m => m.CategoryId).Index(0);
        Map(m => m.Date).Index(1);
        Map(m => m.LabelId).Index(2);
        Map(m => m.WalletId).Index(3);
        Map(m => m.Amount).Index(5);
    }
}