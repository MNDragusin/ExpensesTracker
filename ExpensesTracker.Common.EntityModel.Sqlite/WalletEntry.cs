using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class WalletEntry
{
    [Key] public int EntryId { get; set; }
    [ForeignKey("WalletId")] public string WalletId { get; set; }
    [Required] public DateOnly Date { get; set; }
    [Required] [ForeignKey("CategoryId")] public string CategoryId { get; set; }
    [ForeignKey("LabelId")] public string LabelId { get; set; }
    public float Amount { get; set; }
}