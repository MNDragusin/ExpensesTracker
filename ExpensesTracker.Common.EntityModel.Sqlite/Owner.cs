using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Owner
{
    [Key] public string OwnerId { get; set; }
    [Required] public string Name { get; set; }
    public ICollection<Wallet> Wallets { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<Label> Labels { get; set; }
}