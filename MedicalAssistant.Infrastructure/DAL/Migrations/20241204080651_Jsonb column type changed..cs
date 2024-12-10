using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssistant.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Jsonbcolumntypechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"NotificationHistories\" ALTER COLUMN \"ContentJson\" TYPE jsonb USING \"ContentJson\"::jsonb;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"NotificationHistories\" ALTER COLUMN \"ContentJson\" TYPE text USING \"ContentJson\"::text;"
            );
        }
    }
}
