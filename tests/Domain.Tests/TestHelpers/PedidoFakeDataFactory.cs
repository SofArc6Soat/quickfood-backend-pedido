using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;

namespace Domain.Tests.TestHelpers;

public static class PedidoFakeDataFactory
{
    public static Pedido CriarPedidoValido() => new(ObterGuid(), 1, Guid.NewGuid(), PedidoStatus.Rascunho, 100.00m, DateTime.Now);

    public static Pedido CriarPedidoInvalido() => new(ObterGuid(), 0, null, PedidoStatus.Rascunho, -100.00m, DateTime.MinValue);

    public static PedidoItem CriarPedidoItemValido() => new(ObterGuid(), 2, 10.00m);

    public static PedidoDb CriarPedidoDbValido() => new()
    {
        Id = ObterGuid(),
        NumeroPedido = 1,
        ClienteId = ObterClienteGuid(),
        Status = PedidoStatus.Rascunho.ToString(),
        ValorTotal = 100.00m,
        DataPedido = DateTime.Now,
        Itens =
        [
            new PedidoItemDb
            {
                PedidoId = ObterGuid(),
                ProdutoId = ObterGuid(),
                Quantidade = 2,
                ValorUnitario = 10.00m
            }
        ]
    };

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
    public static Guid ObterClienteGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
    public static Guid ObterNovoGuid() => Guid.Parse("e2a1f1ee-7c54-4b01-90e6-d701748f0852");
}