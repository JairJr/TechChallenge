using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechChallengeApi.Migrations
{
    /// <inheritdoc />
    public partial class DeletandoTabelaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publicacoes_Usuarios_UsuarioId",
                table: "Publicacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Publicacoes_UsuarioId",
                table: "Publicacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_UsuarioId",
                table: "Publicacoes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publicacoes_Usuarios_UsuarioId",
                table: "Publicacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
