using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class recommendationusuagesfixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationUsage_MedicationRecommendation_MedicationReco~",
                table: "RecommendationUsage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationUsage",
                table: "RecommendationUsage");

            migrationBuilder.RenameTable(
                name: "RecommendationUsage",
                newName: "RecommendationUsages");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RecommendationUsages",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationUsage_MedicationRecommendationId",
                table: "RecommendationUsages",
                newName: "IX_RecommendationUsages_MedicationRecommendationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationUsages",
                table: "RecommendationUsages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationUsages_MedicationRecommendation_MedicationRec~",
                table: "RecommendationUsages",
                column: "MedicationRecommendationId",
                principalTable: "MedicationRecommendation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationUsages_MedicationRecommendation_MedicationRec~",
                table: "RecommendationUsages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationUsages",
                table: "RecommendationUsages");

            migrationBuilder.RenameTable(
                name: "RecommendationUsages",
                newName: "RecommendationUsage");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "RecommendationUsage",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationUsages_MedicationRecommendationId",
                table: "RecommendationUsage",
                newName: "IX_RecommendationUsage_MedicationRecommendationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationUsage",
                table: "RecommendationUsage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationUsage_MedicationRecommendation_MedicationReco~",
                table: "RecommendationUsage",
                column: "MedicationRecommendationId",
                principalTable: "MedicationRecommendation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
