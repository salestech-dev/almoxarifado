using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Almoxarifado.Migrations
{
    /// <inheritdoc />
    public partial class AddEstoqueMinimoColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "estoqueMinimo",
                table: "produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estoqueMinimo",
                table: "produtos");
        }
    }
}
