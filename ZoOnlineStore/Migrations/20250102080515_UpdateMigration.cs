using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoOnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Token_Users_UserID",
                table: "Token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Token",
                table: "Token");

            migrationBuilder.RenameTable(
                name: "Token",
                newName: "Tokens");

            migrationBuilder.RenameIndex(
                name: "IX_Token_UserID",
                table: "Tokens",
                newName: "IX_Tokens_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tokens",
                table: "Tokens",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Users_UserID",
                table: "Tokens",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Users_UserID",
                table: "Tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tokens",
                table: "Tokens");

            migrationBuilder.RenameTable(
                name: "Tokens",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_UserID",
                table: "Token",
                newName: "IX_Token_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Token",
                table: "Token",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Token_Users_UserID",
                table: "Token",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
