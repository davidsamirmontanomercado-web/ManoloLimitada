using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManoloLimitada.Migrations
{
    /// <inheritdoc />
    public partial class HacerCedulaUnica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contactos_Cedula",
                table: "Contactos",
                column: "Cedula",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contactos_Cedula",
                table: "Contactos");
        }
    }
}
