using Core.Domain.Entities;

namespace Worker.Dtos.Events
{
    public record PedidoStatusAlteradoEvent : Event
    {
        public string Status { get; set; } = string.Empty;
    }
}
