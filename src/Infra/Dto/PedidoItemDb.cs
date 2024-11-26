using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoItemDb : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public PedidoDb Pedido { get; set; } = new PedidoDb();
    }
}
