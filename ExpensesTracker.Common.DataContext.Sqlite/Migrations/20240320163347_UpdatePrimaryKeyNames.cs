using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Common.DataContext.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrimaryKeyNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Wallets",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Wallets",
                newName: "WalletId");
        }
    }
}
