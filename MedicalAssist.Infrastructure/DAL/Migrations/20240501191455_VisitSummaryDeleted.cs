using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssist.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VisitSummaryDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_VisitSummaries_VisitSummaryId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "VisitSummaries");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_VisitSummaryId",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "VisitSummaryId",
                table: "Recommendations");

            migrationBuilder.AddColumn<Guid>(
                name: "VisitId",
                table: "Recommendations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_VisitId",
                table: "Recommendations",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Visits_VisitId",
                table: "Recommendations",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Visits_VisitId",
                table: "Recommendations");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_VisitId",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "Recommendations");

            migrationBuilder.AddColumn<Guid>(
                name: "VisitSummaryId",
                table: "Recommendations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VisitSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitSummaries_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_VisitSummaryId",
                table: "Recommendations",
                column: "VisitSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitSummaries_VisitId",
                table: "VisitSummaries",
                column: "VisitId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_VisitSummaries_VisitSummaryId",
                table: "Recommendations",
                column: "VisitSummaryId",
                principalTable: "VisitSummaries",
                principalColumn: "Id");
        }
    }
}
