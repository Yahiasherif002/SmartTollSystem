using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTollSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTollV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Invoices",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Invoices",
                newName: "Date");
        }
    }
}
