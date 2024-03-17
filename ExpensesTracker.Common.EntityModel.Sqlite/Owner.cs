using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Owner
{
    [Key] public string OwnerId { get; set; }
    [Required] public string Name { get; set; }
    public virtual ICollection<Wallet> Wallets { get; set; }
    public virtual ICollection<Category> Categories { get; set; }
    public virtual ICollection<Label> Labels { get; set; }
}