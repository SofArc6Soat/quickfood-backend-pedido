using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Pedidos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10, 1"),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 2, nullable: false),
                    DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(40)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(200)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 2, nullable: false),
                    Categoria = table.Column<string>(type: "varchar(20)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidosItens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosItens_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalSchema: "dbo",
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "Categoria", "Descricao", "Nome", "Preco" },
                values: new object[,]
                {
                    { new Guid("111cb598-2df6-41bf-b51b-d4e0f292bda3"), true, "Bebida", "350ml", "PEPSI LATA", 7m },
                    { new Guid("1bb2aef8-97d7-4fb0-94f5-53bff2f3a618"), true, "Acompanhamento", "Anéis de cebola (100g)", "ONION RINGS", 10m },
                    { new Guid("3de0c5e7-787b-4885-8ec8-020866971d3b"), true, "Bebida", "500ml", "ÁGUA", 5m },
                    { new Guid("50ba333a-c804-4d2a-a284-9ff1d147df4e"), true, "Acompanhamento", "Porção individual de batata frita (100g)", "BATATA FRITA", 9m },
                    { new Guid("719bc73f-b90a-4bb0-b6d0-8060ea9f1d4c"), true, "Lanche", "Pão smash, 2 hambúrgueres (150g cada), maionese do feio ,2 queijos cheddar e muito bacon.", "X-DUPLO BACON", 36m },
                    { new Guid("782725ea-70a5-49db-95b2-c4eea841ebca"), true, "Sobremesa", "100g", "SORVETE DE CREME", 12m },
                    { new Guid("b17f425e-e0ff-41cd-92a6-00d78172d7a5"), true, "Sobremesa", "70g", "BROWNIE CHOCOLATE", 10m },
                    { new Guid("c0eab3dc-2ddf-4dde-a64f-392f2412201f"), true, "Bebida", "350ml", "GUARANÁ ANTARCTICA LATA", 7m },
                    { new Guid("c398d290-d1a1-4f2e-a907-ef55e92beef6"), true, "Sobremesa", "100g", "SORVETE DE CHOCOLATE", 12m },
                    { new Guid("e206c795-d6d6-491e-90ed-fdc08e057939"), true, "Sobremesa", "70g", "BROWNIE CHOCOLATE BRANCO", 10m },
                    { new Guid("eee57eb1-1dde-4162-998f-d7148d0c2417"), true, "Lanche", "Pão brioche, hambúrguer (150g) e queijo prato.", "X-BURGUER", 28m },
                    { new Guid("efee2d79-ce89-479a-9667-04f57f9e2e5e"), true, "Lanche", "Pão brioche, hambúrguer (150g), queijo prato, pepino, tomate italiano e alface americana.", "X-SALADA", 30m },
                    { new Guid("fdff63d2-127f-49c5-854a-e47cae8cedb9"), true, "Lanche", "Pão brioche, hambúrguer (150g), queijo prato, bacon, pepino, tomate italiano e alface americana.", "X-BACON", 33m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidosItens_PedidoId",
                schema: "dbo",
                table: "PedidosItens",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidosItens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Produtos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Pedidos",
                schema: "dbo");
        }
    }
}
