using Core.Domain.Entities;
using Worker.Dtos.Events;

namespace Worker.Tests.Dtos;

public class ProdutoEventTests
{
    [Fact]
    public void ProdutoCriadoEvent_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Produto Teste";
        var descricao = "Descrição do Produto Teste";
        var preco = 100.0m;
        var categoria = "Categoria Teste";
        var ativo = true;

        // Act
        var produtoCriadoEvent = new ProdutoCriadoEvent
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            Categoria = categoria,
            Ativo = ativo
        };

        // Assert
        Assert.Equal(id, produtoCriadoEvent.Id);
        Assert.Equal(nome, produtoCriadoEvent.Nome);
        Assert.Equal(descricao, produtoCriadoEvent.Descricao);
        Assert.Equal(preco, produtoCriadoEvent.Preco);
        Assert.Equal(categoria, produtoCriadoEvent.Categoria);
        Assert.True(produtoCriadoEvent.Ativo);
    }

    [Fact]
    public void ProdutoAtualizadoEvent_Should_InheritPropertiesFromProdutoCriadoEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Produto Atualizado";
        var descricao = "Descrição do Produto Atualizado";
        var preco = 150.0m;
        var categoria = "Categoria Atualizada";
        var ativo = false;

        // Act
        var produtoAtualizadoEvent = new ProdutoAtualizadoEvent
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            Categoria = categoria,
            Ativo = ativo
        };

        // Assert
        Assert.Equal(id, produtoAtualizadoEvent.Id);
        Assert.Equal(nome, produtoAtualizadoEvent.Nome);
        Assert.Equal(descricao, produtoAtualizadoEvent.Descricao);
        Assert.Equal(preco, produtoAtualizadoEvent.Preco);
        Assert.Equal(categoria, produtoAtualizadoEvent.Categoria);
        Assert.False(produtoAtualizadoEvent.Ativo);
    }

    [Fact]
    public void ProdutoExcluidoEvent_Should_SetIdCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var produtoExcluidoEvent = new ProdutoExcluidoEvent
        {
            Id = id
        };

        // Assert
        Assert.Equal(id, produtoExcluidoEvent.Id);
    }
}
