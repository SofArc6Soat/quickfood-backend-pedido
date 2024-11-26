namespace Domain.Entities
{
    public class PedidoListaItens(Guid produtoId, int quantidade)
    {
        public Guid ProdutoId { get; set; } = produtoId;
        public int Quantidade { get; set; } = quantidade;
    }
}
