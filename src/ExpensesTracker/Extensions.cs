using ExpensesTracker.Common.Dtos;
using ExpensesTracker.Common.Entities;

namespace ExpensesTracker
{
    public static class Extensions
    {
        public static WalletEntry MapToEntry(this EntryDto dto)
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
        
        public static EntryDto MapToEntryDto(this WalletEntry entry)
        {
            return new EntryDto
            {
                EntryId = entry.EntryId,
                Date = entry.Date,
                Amount = entry.Amount,
                WalletId = entry.WalletId,
                LabelId = entry.LabelId,
                CategoryId = entry.CategoryId,
            };
        }

        public static WalletDto MapToWalletDto(this Wallet wallet)
        {
            return new WalletDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                TotalAmount = wallet.TotalAmount,
                ColorCode = wallet.ColorCode
            };
        }

        public static Wallet MapToWallet(this WalletDto dto, string ownerId)
        {
            return new Wallet
            {
                Id = dto.Id,
                OwnerId = ownerId,
                Name = dto.Name,
                ColorCode = dto.ColorCode,
                TotalAmount = dto.TotalAmount,
                Entries = null
            };
        }

        public static CategoryDto MapToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ColorCode = category.ColorCode
            };
        }
        
        public static Category MapToCategory(this CategoryDto dto, string ownerId)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                ColorCode = dto.ColorCode,
                OwnerId = ownerId
            };
        }

        public static LabelDto MapToLabelDto(this Label label)
        {
            return new LabelDto
            {
                Id = label.Id,
                Name = label.Name,
                ColorCode = label.ColorCode
            };
        }
        
        public static Label MapToLabel(this LabelDto dto, string ownerId)
        {
            return new Label
            {
                Id = dto.Id,
                Name = dto.Name,
                ColorCode = dto.ColorCode,
                OwnerId = ownerId
            };
        }
       
    }
}
