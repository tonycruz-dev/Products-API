using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColorToColour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                table: "Products",
                newName: "Colour");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Colour",
                table: "Products",
                newName: "Color");
        }
    }
}
