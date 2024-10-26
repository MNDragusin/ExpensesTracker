using System;

namespace ExpensesTracker.Models;

public struct WalletViewModel
{
    public required string WalletId { get; set; }
    public required string WalletName {get; set;}
    public required IEnumerable<Entry> Entries { get; set; }
    public required float TotalAmount  {get;set;}
}

public struct Entry
{
    public required string EntryId { get; set; }
    public required DateOnly Date { get; set; }
    public required float Amount { get; set; }
    public required string Category { get; set; }
    public required string Label { get; set; }
}
