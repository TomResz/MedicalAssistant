﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class foreignkeychanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Users_UserId",
                table: "MedicalHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Users_UserId",
                table: "MedicalHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Users_UserId",
                table: "MedicalHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Users_UserId",
                table: "MedicalHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}