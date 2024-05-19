using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssist.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedUserVerificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserVerifications",
                table: "UserVerifications");

            migrationBuilder.DropIndex(
                name: "IX_UserVerifications_UserId",
                table: "UserVerifications");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserVerifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserVerifications",
                table: "UserVerifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserVerifications",
                table: "UserVerifications");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserVerifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserVerifications",
                table: "UserVerifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserVerifications_UserId",
                table: "UserVerifications",
                column: "UserId",
                unique: true);
        }
    }
}
