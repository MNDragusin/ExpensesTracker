using System;
using System.Text.Json.Serialization;

namespace ExpensesTracker.Models;

public struct WalletViewModel
{
    public WalletViewModel(bool hasData = true)
    {
        HasData = hasData;
    }

    [JsonConstructor]
    public WalletViewModel()
    {
    }

    public bool HasData { get; private set; }
    public string WalletId { get; set; }
    public string WalletName { get; set; }
    public IEnumerable<Entry> Entries { get; set; }
    public float TotalAmount { get; set; }
    public string ColorCode { get; set; }
}

public struct Entry
{
    public required string EntryId { get; set; }
    public required DateOnly Date { get; set; }
    public required float Amount { get; set; }
    public required BaseModel Category { get; set; }
    public required BaseModel Label { get; set; }
}

public struct BaseModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string ColorCode { get; set; }
}