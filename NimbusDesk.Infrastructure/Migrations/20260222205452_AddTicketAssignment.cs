using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NimbusDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedToUserId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Tickets");
        }
    }
}
