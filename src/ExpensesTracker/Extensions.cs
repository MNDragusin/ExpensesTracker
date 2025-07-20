using ExpensesTracker.Common.Dtos;
using ExpensesTracker.Common.Entities;

namespace ExpensesTracker
{
    public static class Extensions
    {
        public static WalletEntry MapToEntity(this WalletEntryDto dto)
        {
            return new WalletEntry
            {
                EntryId = dto.EntryId,
                Date = dto.Date,
                Amount = dto.Amount,
                WalletId = dto.WalletId,
                LabelId = dto.LabelId,
                CategoryId = dto.CategoryId
            };
        }

        public static WalletEntryDto MapToDto(this WalletEntry entry)
        {
            return new WalletEntryDto
            {
                Date = entry.Date,
                Amount = entry.Amount,
                WalletId = entry.WalletId,
                LabelId = entry.LabelId,
                CategoryId = entry.CategoryId
            };
        }

        public static Wallet MapToEntity(this WalletDto dto)
        {
            return new Wallet
            {
                Id = dto.Id,
                OwnerId = null,
                Name = dto.Name,
                ColorCode = dto.ColorCode,
                TotalAmount = dto.TotalAmount,
                Entries = null
            };
        }

        public static WalletDto MapToDto(this Wallet wallet)
        {
            return new WalletDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                TotalAmount = wallet.TotalAmount,
                ColorCode = wallet.ColorCode
            };
        }

    }
}
