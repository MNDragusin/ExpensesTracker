using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class WalletEntry
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public string EntryId { get; set; }
    public string WalletId { get; set; }
    [Required] public DateOnly Date { get; set; }
    [Required]public string CategoryId { get; set; }
    public string LabelId { get; set; }
    public float Amount { get; set; }
    
    public virtual Wallet Wallet { get; set; }
    public virtual Category Category { get; set; }
    public virtual Label Label { get; set; }
}