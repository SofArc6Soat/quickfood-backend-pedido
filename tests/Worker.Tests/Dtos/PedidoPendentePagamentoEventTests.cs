using Core.Domain.Entities;
using Worker.Dtos.Events;

namespace Worker.Tests.Dtos;

public class PedidoPendentePagamentoEventTests
{
    [Fact]
    public void PedidoPendentePagamentoEvent_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var status = "PendentePagamento";

        // Act
        var pedidoPendentePagamentoEvent = new PedidoPendentePagamentoEvent
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status
        };

        // Assert
        Assert.Equal(id, pedidoPendentePagamentoEvent.Id);
        Assert.Equal(pedidoId, pedidoPendentePagamentoEvent.PedidoId);
        Assert.Equal(status, pedidoPendentePagamentoEvent.Status);
    }

    [Fact]
    public void PedidoPendentePagamentoEvent_Should_HaveDefaultValues()
    {
        // Act
        var pedidoPendentePagamentoEvent = new PedidoPendentePagamentoEvent();

        // Assert
        Assert.Equal(Guid.Empty, pedidoPendentePagamentoEvent.Id);
        Assert.Equal(Guid.Empty, pedidoPendentePagamentoEvent.PedidoId);
        Assert.Equal("PendentePagamento", pedidoPendentePagamentoEvent.Status);
    }

    [Fact]
    public void PedidoPendentePagamentoEvent_Should_InheritFromEvent()
    {
        // Arrange
        var pedidoPendentePagamentoEvent = new PedidoPendentePagamentoEvent();

        // Act & Assert
        Assert.IsAssignableFrom<Event>(pedidoPendentePagamentoEvent);
    }
}