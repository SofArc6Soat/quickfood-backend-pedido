using Core.Domain.Entities;
using Worker.Dtos.Events;

namespace Worker.Tests.Dtos;

public class PedidoPagoEventTests
{
    [Fact]
    public void PedidoPagoEvent_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var status = "Pago";

        // Act
        var pedidoPagoEvent = new PedidoPagoEvent
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status
        };

        // Assert
        Assert.Equal(id, pedidoPagoEvent.Id);
        Assert.Equal(pedidoId, pedidoPagoEvent.PedidoId);
        Assert.Equal(status, pedidoPagoEvent.Status);
    }

    [Fact]
    public void PedidoPagoEvent_Should_HaveDefaultValues()
    {
        // Act
        var pedidoPagoEvent = new PedidoPagoEvent();

        // Assert
        Assert.Equal(Guid.Empty, pedidoPagoEvent.Id);
        Assert.Equal(Guid.Empty, pedidoPagoEvent.PedidoId);
        Assert.Equal("Pago", pedidoPagoEvent.Status);
    }

    [Fact]
    public void PedidoPagoEvent_Should_InheritFromEvent()
    {
        // Arrange
        var pedidoPagoEvent = new PedidoPagoEvent();

        // Act & Assert
        Assert.IsAssignableFrom<Event>(pedidoPagoEvent);
    }
}