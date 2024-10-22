using Cora.Infra.Repository;
using Dapper;
using Infra.Context;
using Infra.Dto;

namespace Infra.Repositories
{
    public class PedidoRepository(ApplicationDbContext context) : RepositoryGeneric<PedidoDb>(context), IPedidoRepository
    {
        public async Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken)
        {
            var query = @"
                SELECT CONVERT(VARCHAR(MAX),(
                    (SELECT p.Id, p.NumeroPedido, p.ClienteId, p.Status, p.ValorTotal, p.DataPedido, Itens.Nome, Detalhes.Quantidade, Detalhes.ValorUnitario
                    FROM Pedidos p
	                    INNER JOIN PedidosItens Detalhes
		                    ON p.Id = detalhes.PedidoId
	                    INNER JOIN Produtos Itens
		                    ON detalhes.ProdutoId = Itens.Id
                    FOR JSON AUTO)
                ));";

            var result = await GetDbConnection().QueryFirstOrDefaultAsync<string>(query, cancellationToken);

            return !string.IsNullOrEmpty(result) ? result : "[]";
        }

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken)
        {
            var query = @"
                SELECT CONVERT(VARCHAR(MAX),(
                    (SELECT p.Id, p.NumeroPedido, p.Status, p.ValorTotal, p.DataPedido
                    FROM Pedidos p
                    WHERE Status IN ('Pronto', 'EmPreparacao', 'Recebido')
                    ORDER BY 
                        CASE Status
                            WHEN 'Pronto' THEN 1
                            WHEN 'EmPreparacao' THEN 2
                            WHEN 'Recebido' THEN 3
                        END,
                        DataPedido ASC
                    FOR JSON PATH)
                ));";

            var result = await GetDbConnection().QueryFirstOrDefaultAsync<string>(query, cancellationToken);

            return !string.IsNullOrEmpty(result) ? result : "[]";
        }
    }
}
