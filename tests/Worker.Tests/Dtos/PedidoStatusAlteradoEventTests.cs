using Core.Domain.Entities;
using Worker.Dtos.Events;

namespace Worker.Tests.Dtos;

public class PedidoStatusAlteradoEventTests
{
    [Fact]
    public void PedidoStatusAlteradoEvent_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var status = "Status Teste";

        // Act
        var pedidoStatusAlteradoEvent = new PedidoStatusAlteradoEvent
        {
            Id = id,
            Status = status
        };

        // Assert
        Assert.Equal(id, pedidoStatusAlteradoEvent.Id);
        Assert.Equal(status, pedidoStatusAlteradoEvent.Status);
    }

    [Fact]
    public void PedidoStatusAlteradoEvent_Should_HaveDefaultValues()
    {
        // Act
        var pedidoStatusAlteradoEvent = new PedidoStatusAlteradoEvent();

        // Assert
        Assert.Equal(Guid.Empty, pedidoStatusAlteradoEvent.Id);
        Assert.Equal(string.Empty, pedidoStatusAlteradoEvent.Status);
    }

    [Fact]
    public void PedidoStatusAlteradoEvent_Should_InheritFromEvent()
    {
        // Arrange
        var pedidoStatusAlteradoEvent = new PedidoStatusAlteradoEvent();

        // Act & Assert
        Assert.IsAssignableFrom<Event>(pedidoStatusAlteradoEvent);
    }
}