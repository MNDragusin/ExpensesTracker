using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Wallet : BaseDefinition
{
    public virtual ICollection<WalletEntry> Entries { get; set; }
}