using Core.Domain.Entities;

namespace Worker.Dtos.Events
{
    public record ClienteCriadoEvent : Event
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
