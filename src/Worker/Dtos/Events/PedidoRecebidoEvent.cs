using Core.Domain.Entities;

namespace Worker.Dtos.Events
{
    public record PedidoRecebidoEvent : Event
    {
        public int NumeroPedido { get; private set; }
        public Guid? ClienteId { get; private set; }
        public string Status { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataPedido { get; private set; }
        public List<PedidoItemEvent> PedidoItems { get; private set; }

        public PedidoRecebidoEvent(Guid id, int numeroPedido, Guid? clienteId, string status, decimal valorTotal, DateTime dataPedido, List<PedidoItemEvent> itens)
        {
            Id = id;
            NumeroPedido = numeroPedido;
            ClienteId = clienteId;
            Status = status;
            ValorTotal = valorTotal;
            DataPedido = dataPedido;
            PedidoItems = itens;
        }
    }

    public record PedidoItemEvent(Guid Id, Guid PedidoId, Guid ProdutoId, int Quantidade, decimal ValorUnitario)
    {
        public Guid Id { get; private set; } = Id;
        public Guid PedidoId { get; private set; } = PedidoId;
        public Guid ProdutoId { get; private set; } = ProdutoId;
        public int Quantidade { get; private set; } = Quantidade;
        public decimal ValorUnitario { get; private set; } = ValorUnitario;
    }
}
