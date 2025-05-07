using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTollSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateToll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedsConfirmation",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedsConfirmation",
                table: "Vehicles");
        }
    }
}
