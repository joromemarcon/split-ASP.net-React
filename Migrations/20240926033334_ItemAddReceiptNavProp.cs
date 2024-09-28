using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace split_api.Migrations
{
    /// <inheritdoc />
    public partial class ItemAddReceiptNavProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Items_ReceiptId",
                table: "Items",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Receipts_ReceiptId",
                table: "Items",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Receipts_ReceiptId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ReceiptId",
                table: "Items");
        }
    }
}
