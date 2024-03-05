using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Wallet
{
    [Key] public string WalletId { get; set; }
    [ForeignKey("OwnerId")] public string OwnerId { get; set; }
    [Required] public string Name { get; set; }
    private ICollection<WalletEntry> Entries { get; set; }
}