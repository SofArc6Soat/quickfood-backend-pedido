using Core.Domain.Data;
using Domain.Tests.TestHelpers;
using Infra.Context;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infra.Tests.Repositories;

public class ProdutoRepositoryTests
{
    private ProdutoRepository _produtoRepository;
    private ApplicationDbContext _context;

    private void SetupInMemoryDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _produtoRepository = new ProdutoRepository(_context);
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_Should_ReturnAllActiveProducts()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produtos = new List<ProdutoDb>
            {
                new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto 1", Descricao = "Descrição 1", Preco = 10.0m, Categoria = "Categoria 1", Ativo = true },
                new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto 2", Descricao = "Descrição 2", Preco = 20.0m, Categoria = "Categoria 2", Ativo = true },
                new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto 3", Descricao = "Descrição 3", Preco = 30.0m, Categoria = "Categoria 3", Ativo = false }
            };
        await _context.Set<ProdutoDb>().AddRangeAsync(produtos);
        await _context.SaveChangesAsync();

        // Act
        var result = await _produtoRepository.ObterTodosProdutosAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Nome == "Produto 1");
        Assert.Contains(result, p => p.Nome == "Produto 2");
        Assert.DoesNotContain(result, p => p.Nome == "Produto 3");
    }

    [Fact]
    public async Task FindByIdAsync_Should_ReturnProduct_When_ProductExists()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produtoId = Guid.NewGuid();
        var produto = new ProdutoDb { Id = produtoId, Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 100.0m, Categoria = "Categoria Teste", Ativo = true };
        await _context.Set<ProdutoDb>().AddAsync(produto);
        await _context.SaveChangesAsync();

        // Act
        var result = await _produtoRepository.FindByIdAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(produtoId, result.Id);
    }

    [Fact]
    public async Task FindByIdAsync_Should_ReturnNull_When_ProductDoesNotExist()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produtoId = Guid.NewGuid();

        // Act
        var result = await _produtoRepository.FindByIdAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertAsync_Should_AddProduct()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produto = new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 100.0m, Categoria = "Categoria Teste", Ativo = true };

        // Act
        await _produtoRepository.InsertAsync(produto, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Set<ProdutoDb>().FindAsync(produto.Id);
        Assert.NotNull(result);
        Assert.Equal(produto.Nome, result.Nome);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateProduct()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produto = new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 100.0m, Categoria = "Categoria Teste", Ativo = true };
        await _context.Set<ProdutoDb>().AddAsync(produto);
        await _context.SaveChangesAsync();

        produto.Nome = "Produto Atualizado";

        // Act
        await _produtoRepository.UpdateAsync(produto, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Set<ProdutoDb>().FindAsync(produto.Id);
        Assert.NotNull(result);
        Assert.Equal("Produto Atualizado", result.Nome);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveProduct()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produto = new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 100.0m, Categoria = "Categoria Teste", Ativo = true };
        await _context.Set<ProdutoDb>().AddAsync(produto);
        await _context.SaveChangesAsync();

        // Act
        await _produtoRepository.DeleteAsync(produto.Id, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Set<ProdutoDb>().FindAsync(produto.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task CommitAsync_Should_CommitTransaction()
    {
        // Arrange
        SetupInMemoryDatabase();

        var produto = new ProdutoDb { Id = Guid.NewGuid(), Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 100.0m, Categoria = "Categoria Teste", Ativo = true };
        await _context.Set<ProdutoDb>().AddAsync(produto);

        // Act
        var result = await _context.CommitAsync(CancellationToken.None);

        // Assert
        Assert.True(result);
        var savedProduct = await _context.Set<ProdutoDb>().FindAsync(produto.Id);
        Assert.NotNull(savedProduct);
    }
}
