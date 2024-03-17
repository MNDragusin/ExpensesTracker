using ExpensesTracker.Common.EntityModel.Sqlite;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Server.Services;

public interface IDataImporter
{
    public Task<IEnumerable<WalletEntry>> ParseCSVAsync(IBrowserFile file, string userId);
}