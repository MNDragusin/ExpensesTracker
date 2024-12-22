using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities;

public class BaseDefinition
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public required string Id { get; set; }
    public required string OwnerId { get; set; }
    [Required] public required string Name { get; set; }
    public string? ColorCode { get; set; }
}