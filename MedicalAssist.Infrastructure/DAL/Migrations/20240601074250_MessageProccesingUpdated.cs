using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssist.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MessageProccesingUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                schema: "MessageProcessing",
                table: "OutboxMessage",
                newName: "ErrorMessageJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorMessageJson",
                schema: "MessageProcessing",
                table: "OutboxMessage",
                newName: "ErrorMessage");
        }
    }
}
