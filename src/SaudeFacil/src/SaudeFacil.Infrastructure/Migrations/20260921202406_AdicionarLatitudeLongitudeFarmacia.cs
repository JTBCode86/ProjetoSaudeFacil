using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaudeFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarLatitudeLongitudeFarmacia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco_Longitude",
                table: "Farmacias",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Endereco_Latitude",
                table: "Farmacias",
                newName: "Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Farmacias",
                newName: "Endereco_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Farmacias",
                newName: "Endereco_Latitude");
        }
    }
}
