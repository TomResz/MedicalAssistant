using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class historyindexsearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_DiseaseName_SymptomDescription",
                table: "MedicalHistories",
                columns: new[] { "DiseaseName", "SymptomDescription" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_DiseaseName_SymptomDescription",
                table: "MedicalHistories");
        }
    }
}
