namespace ExpensesTracker.Common.Dtos;

public class WalletEntryDto
{
    public string? EntryId { get; set; } // Nullable to allow for new entries without an ID
    public DateOnly Date { get; set; }
    public float Amount { get; set; }
    public string WalletId { get; set; }
    public string LabelId { get; set; }
    public string CategoryId { get; set; }
}
