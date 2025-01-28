using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace split_api.Migrations
{
    /// <inheritdoc />
    public partial class reloadingDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SplitUserId",
                table: "CustomerReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReceipts_ReceiptId",
                table: "CustomerReceipts",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReceipts_SplitUserId",
                table: "CustomerReceipts",
                column: "SplitUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReceipts_Receipts_ReceiptId",
                table: "CustomerReceipts",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReceipts_SplitUsers_SplitUserId",
                table: "CustomerReceipts",
                column: "SplitUserId",
                principalTable: "SplitUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReceipts_Receipts_ReceiptId",
                table: "CustomerReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReceipts_SplitUsers_SplitUserId",
                table: "CustomerReceipts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReceipts_ReceiptId",
                table: "CustomerReceipts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReceipts_SplitUserId",
                table: "CustomerReceipts");

            migrationBuilder.DropColumn(
                name: "SplitUserId",
                table: "CustomerReceipts");
        }
    }
}
