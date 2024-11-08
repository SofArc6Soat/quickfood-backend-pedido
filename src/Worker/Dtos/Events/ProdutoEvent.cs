using Core.Domain.Entities;

namespace Worker.Dtos.Events
{
    public record ProdutoCriadoEvent : Event
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }

    public record ProdutoAtualizadoEvent : ProdutoCriadoEvent
    {

    }

    public record ProdutoExcluidoEvent : Event
    {

    }
}
