using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedVisitNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SimpleId = table.Column<string>(type: "text", nullable: false),
                    ScheduledDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitNotifications_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitNotifications_VisitId",
                table: "VisitNotifications",
                column: "VisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitNotifications");
        }
    }
}
