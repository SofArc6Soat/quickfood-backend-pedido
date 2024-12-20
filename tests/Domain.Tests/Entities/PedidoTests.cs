﻿using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities
{
    public class PedidoTests
    {
        [Fact]
        public void AdicionarItem_DeveAdicionarItemNovo()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, 2, 50.00m);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            pedido.PedidoItems.Should().Contain(pedidoItem); // Verifica se o item foi adicionado
            pedido.ValorTotal.Should().Be(pedidoItem.CalcularValor()); // Verifica se o valor total está correto
        }

        [Fact]
        public void AdicionarItem_DeveAtualizarItemExistente()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente = new PedidoItem(produtoId, 2, 50.00m);
            pedido.AdicionarItem(pedidoItemExistente); // Adiciona o item inicialmente

            var pedidoItemAtualizado = new PedidoItem(produtoId, 3, 50.00m); // Novo item com a mesma ID de produto, mas com quantidade diferente

            // Act
            pedido.AdicionarItem(pedidoItemAtualizado);

            // Assert
            var itemAtualizado = pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == produtoId);
            itemAtualizado.Should().NotBeNull();
            itemAtualizado!.Quantidade.Should().Be(5); // Verifica se a quantidade foi atualizada corretamente
            pedido.ValorTotal.Should().Be(itemAtualizado.CalcularValor()); // Verifica se o valor total foi calculado corretamente
        }
    }
}