using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class WalletEntry
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public required string EntryId { get; set; }
    public required string WalletId { get; set; }
    [Required] public DateOnly Date { get; set; }
    [Required] public required string CategoryId { get; set; }
    public required string LabelId { get; set; }
    public float Amount { get; set; }
}