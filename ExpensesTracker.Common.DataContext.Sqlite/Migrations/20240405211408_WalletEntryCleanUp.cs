using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Common.DataContext.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class WalletEntryCleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletEntries_Categories_CategoryId",
                table: "WalletEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletEntries_Labels_LabelId",
                table: "WalletEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletEntries_Wallets_WalletId",
                table: "WalletEntries");

            migrationBuilder.DropIndex(
                name: "IX_WalletEntries_CategoryId",
                table: "WalletEntries");

            migrationBuilder.DropIndex(
                name: "IX_WalletEntries_LabelId",
                table: "WalletEntries");

            migrationBuilder.DropIndex(
                name: "IX_WalletEntries_WalletId",
                table: "WalletEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WalletEntries_CategoryId",
                table: "WalletEntries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletEntries_LabelId",
                table: "WalletEntries",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletEntries_WalletId",
                table: "WalletEntries",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletEntries_Categories_CategoryId",
                table: "WalletEntries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletEntries_Labels_LabelId",
                table: "WalletEntries",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletEntries_Wallets_WalletId",
                table: "WalletEntries",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
