using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Wallet
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public string WalletId { get; set; }
    public string OwnerId { get; set; }
    [Required] public string Name { get; set; }
    public virtual ICollection<WalletEntry> Entries { get; set; }
}