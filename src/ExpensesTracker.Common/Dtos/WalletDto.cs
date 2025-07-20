
namespace ExpensesTracker.Common.Dtos
{
    public class WalletDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? ColorCode { get; set; }
        public IEnumerable<WalletDto> Entries { get; set; }
        public float TotalAmount { get; set; }
    }
}
