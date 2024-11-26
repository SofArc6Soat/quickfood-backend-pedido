using Core.Domain.Entities;

namespace Domain.Entities
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public string Categoria { get; private set; }
        public bool Ativo { get; private set; }

        public Produto(Guid id, string nome, string descricao, decimal preco, string categoria, bool ativo)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Categoria = categoria;
            Ativo = ativo;
        }
    }
}
