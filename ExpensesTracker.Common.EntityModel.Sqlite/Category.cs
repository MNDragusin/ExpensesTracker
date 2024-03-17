using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Category
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public string Id { get; set; } 
    public string OwnerId { get; set; }
    [Required] public string Name { get; set; }
}