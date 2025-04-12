using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetoTopicos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    EnderecoID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    logradouro = table.Column<string>(type: "TEXT", nullable: false),
                    cidade = table.Column<string>(type: "TEXT", nullable: false),
                    uf = table.Column<string>(type: "TEXT", nullable: false),
                    cep = table.Column<string>(type: "TEXT", nullable: false),
                    numero = table.Column<int>(type: "INTEGER", nullable: false),
                    dataCricao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.EnderecoID);
                });

            migrationBuilder.CreateTable(
                name: "Abrigos",
                columns: table => new
                {
                    abrigoID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    qtdPets = table.Column<int>(type: "INTEGER", nullable: false),
                    EnderecoID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abrigos", x => x.abrigoID);
                    table.ForeignKey(
                        name: "FK_Abrigos_Enderecos_EnderecoID",
                        column: x => x.EnderecoID,
                        principalTable: "Enderecos",
                        principalColumn: "EnderecoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    petID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    idade = table.Column<int>(type: "INTEGER", nullable: false),
                    porte = table.Column<string>(type: "TEXT", nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    AbrigoID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.petID);
                    table.ForeignKey(
                        name: "FK_Pets_Abrigos_AbrigoID",
                        column: x => x.AbrigoID,
                        principalTable: "Abrigos",
                        principalColumn: "abrigoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adocoes",
                columns: table => new
                {
                    AdocaoID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AbrigoID = table.Column<int>(type: "INTEGER", nullable: false),
                    petID = table.Column<int>(type: "INTEGER", nullable: false),
                    realizadaEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    cpfTutor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adocoes", x => x.AdocaoID);
                    table.ForeignKey(
                        name: "FK_Adocoes_Abrigos_AbrigoID",
                        column: x => x.AbrigoID,
                        principalTable: "Abrigos",
                        principalColumn: "abrigoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adocoes_Pets_petID",
                        column: x => x.petID,
                        principalTable: "Pets",
                        principalColumn: "petID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abrigos_EnderecoID",
                table: "Abrigos",
                column: "EnderecoID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adocoes_AbrigoID",
                table: "Adocoes",
                column: "AbrigoID");

            migrationBuilder.CreateIndex(
                name: "IX_Adocoes_petID",
                table: "Adocoes",
                column: "petID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_AbrigoID",
                table: "Pets",
                column: "AbrigoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adocoes");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Abrigos");

            migrationBuilder.DropTable(
                name: "Enderecos");
        }
    }
}
