using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoDb : Entity
    {
        public int NumeroPedido { get; set; }
        public Guid? ClienteId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }

        public List<PedidoItemDb> Itens { get; set; }

        public PedidoDb() => Itens = [];
    }
}
