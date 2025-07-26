
namespace ExpensesTracker.Common.Dtos
{
    public class WalletDto : BaseDto
    {
        public IEnumerable<EntryDto>? Entries { get; set; }
        public float TotalAmount { get; set; }
    }
}
