using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaudeFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCpfCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Clientes");
        }
    }
}
