using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comandas.Api.Migrations
{
    /// <inheritdoc />
    public partial class change_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComandaItem_CardapioItem_CardapioItemId",
                table: "ComandaItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ComandaItem_Comanda_ComandaId",
                table: "ComandaItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCozinha_Comanda_ComandaId",
                table: "PedidoCozinha");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCozinhaItem_ComandaItem_ComandaItemId",
                table: "PedidoCozinhaItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCozinhaItem_PedidoCozinha_PedidoCozinhaId",
                table: "PedidoCozinhaItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoCozinhaItem",
                table: "PedidoCozinhaItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoCozinha",
                table: "PedidoCozinha");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComandaItem",
                table: "ComandaItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comanda",
                table: "Comanda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardapioItem",
                table: "CardapioItem");

            migrationBuilder.RenameTable(
                name: "PedidoCozinhaItem",
                newName: "PedidoCozinhaItems");

            migrationBuilder.RenameTable(
                name: "PedidoCozinha",
                newName: "PedidoCozinhas");

            migrationBuilder.RenameTable(
                name: "ComandaItem",
                newName: "ComandaItems");

            migrationBuilder.RenameTable(
                name: "Comanda",
                newName: "Comandas");

            migrationBuilder.RenameTable(
                name: "CardapioItem",
                newName: "CardapioItems");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoCozinhaItem_PedidoCozinhaId",
                table: "PedidoCozinhaItems",
                newName: "IX_PedidoCozinhaItems_PedidoCozinhaId");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoCozinhaItem_ComandaItemId",
                table: "PedidoCozinhaItems",
                newName: "IX_PedidoCozinhaItems_ComandaItemId");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoCozinha_ComandaId",
                table: "PedidoCozinhas",
                newName: "IX_PedidoCozinhas_ComandaId");

            migrationBuilder.RenameIndex(
                name: "IX_ComandaItem_ComandaId",
                table: "ComandaItems",
                newName: "IX_ComandaItems_ComandaId");

            migrationBuilder.RenameIndex(
                name: "IX_ComandaItem_CardapioItemId",
                table: "ComandaItems",
                newName: "IX_ComandaItems_CardapioItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoCozinhaItems",
                table: "PedidoCozinhaItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoCozinhas",
                table: "PedidoCozinhas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComandaItems",
                table: "ComandaItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comandas",
                table: "Comandas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardapioItems",
                table: "CardapioItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Mesas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroMesa = table.Column<int>(type: "int", nullable: false),
                    SituacaoMesa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_ComandaItems_CardapioItems_CardapioItemId",
                table: "ComandaItems",
                column: "CardapioItemId",
                principalTable: "CardapioItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComandaItems_Comandas_ComandaId",
                table: "ComandaItems",
                column: "ComandaId",
                principalTable: "Comandas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCozinhaItems_ComandaItems_ComandaItemId",
                table: "PedidoCozinhaItems",
                column: "ComandaItemId",
                principalTable: "ComandaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCozinhaItems_PedidoCozinhas_PedidoCozinhaId",
                table: "PedidoCozinhaItems",
                column: "PedidoCozinhaId",
                principalTable: "PedidoCozinhas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCozinhas_Comandas_ComandaId",
                table: "PedidoCozinhas",
                column: "ComandaId",
                principalTable: "Comandas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComandaItems_CardapioItems_CardapioItemId",
                table: "ComandaItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ComandaItems_Comandas_ComandaId",
                table: "ComandaItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCozinhaItems_ComandaItems_ComandaItemId",
                table: "PedidoCozinhaItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCozinhaItems_PedidoCozinhas_PedidoCozinhaId",
                table: "PedidoCozinhaItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCozinhas_Comandas_ComandaId",
                table: "PedidoCozinhas");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoCozinhas",
                table: "PedidoCozinhas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoCozinhaItems",
                table: "PedidoCozinhaItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comandas",
                table: "Comandas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComandaItems",
                table: "ComandaItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardapioItems",
                table: "CardapioItems");

            migrationBuilder.RenameTable(
                name: "PedidoCozinhas",
                newName: "PedidoCozinha");

            migrationBuilder.RenameTable(
                name: "PedidoCozinhaItems",
                newName: "PedidoCozinhaItem");

            migrationBuilder.RenameTable(
                name: "Comandas",
                newName: "Comanda");

            migrationBuilder.RenameTable(
                name: "ComandaItems",
                newName: "ComandaItem");

            migrationBuilder.RenameTable(
                name: "CardapioItems",
                newName: "CardapioItem");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoCozinhas_ComandaId",
                table: "PedidoCozinha",
                newName: "IX_PedidoCozinha_ComandaId");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoCozinhaItems_PedidoCozinhaId",
                table: "PedidoCozinhaItem",
                newName: "IX_PedidoCozinhaItem_PedidoCozinhaId");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoCozinhaItems_ComandaItemId",
                table: "PedidoCozinhaItem",
                newName: "IX_PedidoCozinhaItem_ComandaItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ComandaItems_ComandaId",
                table: "ComandaItem",
                newName: "IX_ComandaItem_ComandaId");

            migrationBuilder.RenameIndex(
                name: "IX_ComandaItems_CardapioItemId",
                table: "ComandaItem",
                newName: "IX_ComandaItem_CardapioItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoCozinha",
                table: "PedidoCozinha",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoCozinhaItem",
                table: "PedidoCozinhaItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comanda",
                table: "Comanda",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComandaItem",
                table: "ComandaItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardapioItem",
                table: "CardapioItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComandaItem_CardapioItem_CardapioItemId",
                table: "ComandaItem",
                column: "CardapioItemId",
                principalTable: "CardapioItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComandaItem_Comanda_ComandaId",
                table: "ComandaItem",
                column: "ComandaId",
                principalTable: "Comanda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCozinha_Comanda_ComandaId",
                table: "PedidoCozinha",
                column: "ComandaId",
                principalTable: "Comanda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCozinhaItem_ComandaItem_ComandaItemId",
                table: "PedidoCozinhaItem",
                column: "ComandaItemId",
                principalTable: "ComandaItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCozinhaItem_PedidoCozinha_PedidoCozinhaId",
                table: "PedidoCozinhaItem",
                column: "PedidoCozinhaId",
                principalTable: "PedidoCozinha",
                principalColumn: "Id");
        }
    }
}
