using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Owner
{
    [Key] public required string OwnerId { get; set; }
    [Required] public required string Name { get; set; }
    public virtual required ICollection<Wallet> Wallets { get; set; }
    public virtual required ICollection<Category> Categories { get; set; }
    public virtual required ICollection<Label> Labels { get; set; }
}