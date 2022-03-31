using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cumbuca.Migrations
{
    public partial class ModifyUserFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_User_ContaEnvianteFK",
                table: "Transacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_User_ContaRecebedoraFK",
                table: "Transacoes");

            migrationBuilder.RenameColumn(
                name: "ContaRecebedoraFK",
                table: "Transacoes",
                newName: "ContaRecebedora");

            migrationBuilder.RenameColumn(
                name: "ContaEnvianteFK",
                table: "Transacoes",
                newName: "ContaEnviante");

            migrationBuilder.RenameIndex(
                name: "IX_Transacoes_ContaRecebedoraFK",
                table: "Transacoes",
                newName: "IX_Transacoes_ContaRecebedora");

            migrationBuilder.RenameIndex(
                name: "IX_Transacoes_ContaEnvianteFK",
                table: "Transacoes",
                newName: "IX_Transacoes_ContaEnviante");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_User_ContaEnviante",
                table: "Transacoes",
                column: "ContaEnviante",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_User_ContaRecebedora",
                table: "Transacoes",
                column: "ContaRecebedora",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_User_ContaEnviante",
                table: "Transacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_User_ContaRecebedora",
                table: "Transacoes");

            migrationBuilder.RenameColumn(
                name: "ContaRecebedora",
                table: "Transacoes",
                newName: "ContaRecebedoraFK");

            migrationBuilder.RenameColumn(
                name: "ContaEnviante",
                table: "Transacoes",
                newName: "ContaEnvianteFK");

            migrationBuilder.RenameIndex(
                name: "IX_Transacoes_ContaRecebedora",
                table: "Transacoes",
                newName: "IX_Transacoes_ContaRecebedoraFK");

            migrationBuilder.RenameIndex(
                name: "IX_Transacoes_ContaEnviante",
                table: "Transacoes",
                newName: "IX_Transacoes_ContaEnvianteFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_User_ContaEnvianteFK",
                table: "Transacoes",
                column: "ContaEnvianteFK",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_User_ContaRecebedoraFK",
                table: "Transacoes",
                column: "ContaRecebedoraFK",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
