using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssist.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraColumnToRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Medicine_TimeOfDay",
                table: "Recommendations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medicine_TimeOfDay",
                table: "Recommendations");
        }
    }
}
