using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedPrimaryIdTokenHolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenHolders",
                table: "TokenHolders");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TokenHolders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenHolders",
                table: "TokenHolders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TokenHolders_UserId",
                table: "TokenHolders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenHolders",
                table: "TokenHolders");

            migrationBuilder.DropIndex(
                name: "IX_TokenHolders_UserId",
                table: "TokenHolders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TokenHolders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenHolders",
                table: "TokenHolders",
                column: "UserId");
        }
    }
}
