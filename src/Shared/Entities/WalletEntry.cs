using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities;

public class WalletEntry
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public required string EntryId { get; set; }
    [Required] public DateOnly Date { get; set; }
    public float Amount { get; set; }
    
    public required string WalletId { get; set; }
    public Wallet? Wallet { get; set; }
    
    public required string LabelId { get; set; }
    public Label? Label { get; set; }
    
    [Required] public required string CategoryId { get; set; }
    public Category? Category { get; set; }

    public static WalletEntry NewEmptyWalletEntry()
    {
        return new WalletEntry
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            EntryId = null,
            WalletId = string.Empty,
            LabelId = string.Empty,
            CategoryId = string.Empty
        };
    }
}