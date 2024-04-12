using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class WalletEntry
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public string EntryId { get; set; }
    public string WalletId { get; set; }
    [Required] public DateOnly Date { get; set; }
    [Required] public string CategoryId { get; set; }
    public string LabelId { get; set; }
    public float Amount { get; set; }
}