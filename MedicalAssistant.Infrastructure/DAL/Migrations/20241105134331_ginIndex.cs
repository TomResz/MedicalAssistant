using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ginIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitType_VisitDescription_DoctorName",
                table: "Visits",
                columns: new[] { "VisitType", "VisitDescription", "DoctorName" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Visits_VisitType_VisitDescription_DoctorName",
                table: "Visits");
        }
    }
}
