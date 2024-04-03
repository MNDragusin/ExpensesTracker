using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Wallet : BaseDefinition
{
    [NotMapped]
    public virtual IEnumerable<WalletEntry> Entries { get; set; }
    [NotMapped]
    public virtual float TotalAmount { get; set; }
}