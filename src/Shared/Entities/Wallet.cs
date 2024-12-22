
namespace Shared.Entities;

public class Wallet : BaseDefinition
{
    public virtual required IEnumerable<WalletEntry> Entries { get; set; }
    public virtual float TotalAmount { get; set; }
}