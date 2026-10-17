using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaudeFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Endereco_Latitude",
                table: "Farmacias",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Endereco_Longitude",
                table: "Farmacias",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Endereco_Latitude",
                table: "Clientes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Endereco_Longitude",
                table: "Clientes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco_Latitude",
                table: "Farmacias");

            migrationBuilder.DropColumn(
                name: "Endereco_Longitude",
                table: "Farmacias");

            migrationBuilder.DropColumn(
                name: "Endereco_Latitude",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_Longitude",
                table: "Clientes");
        }
    }
}
