using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAssist.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MessageProccesingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages");

            migrationBuilder.EnsureSchema(
                name: "MessageProcessing");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                newName: "OutboxMessage",
                newSchema: "MessageProcessing");

            migrationBuilder.RenameColumn(
                name: "Content",
                schema: "MessageProcessing",
                table: "OutboxMessage",
                newName: "ContentJson");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessage",
                schema: "MessageProcessing",
                table: "OutboxMessage",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessage",
                schema: "MessageProcessing",
                table: "OutboxMessage");

            migrationBuilder.RenameTable(
                name: "OutboxMessage",
                schema: "MessageProcessing",
                newName: "OutboxMessages");

            migrationBuilder.RenameColumn(
                name: "ContentJson",
                table: "OutboxMessages",
                newName: "Content");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages",
                column: "Id");
        }
    }
}
