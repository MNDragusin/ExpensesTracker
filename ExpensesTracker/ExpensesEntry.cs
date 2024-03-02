using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Server.Controllers;

public class ExpensesEntry
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string WalletId { get; set; }
    public string Category { get; set; }
    public string Label { get; set; }
    public float Amount { get; set; }
}