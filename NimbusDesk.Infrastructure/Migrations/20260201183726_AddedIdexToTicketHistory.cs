using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NimbusDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdexToTicketHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TicketHistory_TicketId_ChangedAt",
                table: "TicketHistory",
                columns: new[] { "TicketId", "ChangedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketHistory_TicketId_ChangedAt",
                table: "TicketHistory");
        }
    }
}
