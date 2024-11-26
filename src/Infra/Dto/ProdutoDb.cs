using Core.Domain.Entities;

namespace Infra.Dto
{
    public class ProdutoDb : Entity
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
