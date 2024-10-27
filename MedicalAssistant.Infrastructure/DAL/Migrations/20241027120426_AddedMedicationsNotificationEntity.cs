using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMedicationsNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimpleId",
                table: "VisitNotifications",
                newName: "JobId");

            migrationBuilder.CreateTable(
                name: "MedicationRecommendationsNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicationRecommendationId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TriggerTimeUtc = table.Column<TimeSpan>(type: "TIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationRecommendationsNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationRecommendationsNotifications_MedicationRecommenda~",
                        column: x => x.MedicationRecommendationId,
                        principalTable: "MedicationRecommendation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationRecommendationsNotifications_MedicationRecommenda~",
                table: "MedicationRecommendationsNotifications",
                column: "MedicationRecommendationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicationRecommendationsNotifications");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "VisitNotifications",
                newName: "SimpleId");
        }
    }
}
