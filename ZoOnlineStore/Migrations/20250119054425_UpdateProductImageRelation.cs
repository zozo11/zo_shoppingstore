using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoOnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductID",
                table: "Inventories");

            migrationBuilder.AddColumn<int>(
                name: "InventoryID",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_InventoryID",
                table: "Products",
                column: "InventoryID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Inventories_InventoryID",
                table: "Products",
                column: "InventoryID",
                principalTable: "Inventories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Inventories_InventoryID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_InventoryID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InventoryID",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductID",
                table: "Inventories",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
