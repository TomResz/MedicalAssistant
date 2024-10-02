using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renamedtokenholdertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenHolder_Users_UserId",
                table: "TokenHolder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenHolder",
                table: "TokenHolder");

            migrationBuilder.RenameTable(
                name: "TokenHolder",
                newName: "TokenHolders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenHolders",
                table: "TokenHolders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TokenHolders_Users_UserId",
                table: "TokenHolders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenHolders_Users_UserId",
                table: "TokenHolders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenHolders",
                table: "TokenHolders");

            migrationBuilder.RenameTable(
                name: "TokenHolders",
                newName: "TokenHolder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenHolder",
                table: "TokenHolder",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TokenHolder_Users_UserId",
                table: "TokenHolder",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
