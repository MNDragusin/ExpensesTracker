using System;
using ExpensesTracker.Common.EntityModel.Sqlite;

namespace ExpensesTracker.Models;

public class WalletsListViewModel
{
    public List<WalletViewModel> Wallets { get; set; }
}
