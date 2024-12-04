using Worker.Dtos.Events;

namespace Worker.Tests.Dtos;

public class PedidoRecebidoEventTests
{
    [Fact]
    public void PedidoRecebidoEvent_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var numeroPedido = 12345;
        var clienteId = Guid.NewGuid();
        var status = "Recebido";
        var valorTotal = 250.75m;
        var dataPedido = DateTime.UtcNow;
        var pedidoItems = new List<PedidoItemEvent>
            {
                new PedidoItemEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2, 50.00m),
                new PedidoItemEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, 150.75m)
            };

        // Act
        var pedidoRecebidoEvent = new PedidoRecebidoEvent(id, numeroPedido, clienteId, status, valorTotal, dataPedido, pedidoItems);

        // Assert
        Assert.Equal(id, pedidoRecebidoEvent.Id);
        Assert.Equal(numeroPedido, pedidoRecebidoEvent.NumeroPedido);
        Assert.Equal(clienteId, pedidoRecebidoEvent.ClienteId);
        Assert.Equal(status, pedidoRecebidoEvent.Status);
        Assert.Equal(valorTotal, pedidoRecebidoEvent.ValorTotal);
        Assert.Equal(dataPedido, pedidoRecebidoEvent.DataPedido);
        Assert.Equal(pedidoItems, pedidoRecebidoEvent.PedidoItems);
    }

    [Fact]
    public void PedidoRecebidoEvent_Should_HaveDefaultValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var numeroPedido = 0;
        Guid? clienteId = null;
        var status = string.Empty;
        var valorTotal = 0m;
        var dataPedido = default(DateTime);
        var pedidoItems = new List<PedidoItemEvent>();

        // Act
        var pedidoRecebidoEvent = new PedidoRecebidoEvent(id, numeroPedido, clienteId, status, valorTotal, dataPedido, pedidoItems);

        // Assert
        Assert.Equal(id, pedidoRecebidoEvent.Id);
        Assert.Equal(numeroPedido, pedidoRecebidoEvent.NumeroPedido);
        Assert.Equal(clienteId, pedidoRecebidoEvent.ClienteId);
        Assert.Equal(status, pedidoRecebidoEvent.Status);
        Assert.Equal(valorTotal, pedidoRecebidoEvent.ValorTotal);
        Assert.Equal(dataPedido, pedidoRecebidoEvent.DataPedido);
        Assert.Equal(pedidoItems, pedidoRecebidoEvent.PedidoItems);
    }

    [Fact]
    public void PedidoItemEvent_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();
        var quantidade = 3;
        var valorUnitario = 75.50m;

        // Act
        var pedidoItemEvent = new PedidoItemEvent(id, pedidoId, produtoId, quantidade, valorUnitario);

        // Assert
        Assert.Equal(id, pedidoItemEvent.Id);
        Assert.Equal(pedidoId, pedidoItemEvent.PedidoId);
        Assert.Equal(produtoId, pedidoItemEvent.ProdutoId);
        Assert.Equal(quantidade, pedidoItemEvent.Quantidade);
        Assert.Equal(valorUnitario, pedidoItemEvent.ValorUnitario);
    }
}