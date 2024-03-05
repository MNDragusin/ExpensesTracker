using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Common.EntityModel.Sqlite;

public class Label
{
    [Key] public string Id { get; set; }
    [ForeignKey("OwnerId")] public string OwnerId { get; set; }
    [Required] public string Name { get; set; }
}