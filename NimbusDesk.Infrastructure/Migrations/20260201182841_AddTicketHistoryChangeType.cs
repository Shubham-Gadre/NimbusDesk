using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NimbusDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketHistoryChangeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromStatus",
                table: "TicketHistory");

            migrationBuilder.RenameColumn(
                name: "ToStatus",
                table: "TicketHistory",
                newName: "ChangeType");

            migrationBuilder.AddColumn<string>(
                name: "FromValue",
                table: "TicketHistory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToValue",
                table: "TicketHistory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromValue",
                table: "TicketHistory");

            migrationBuilder.DropColumn(
                name: "ToValue",
                table: "TicketHistory");

            migrationBuilder.RenameColumn(
                name: "ChangeType",
                table: "TicketHistory",
                newName: "ToStatus");

            migrationBuilder.AddColumn<string>(
                name: "FromStatus",
                table: "TicketHistory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
