using Dapper;
using Infra.Context;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;
using System.Linq.Expressions;

namespace Infra.Tests.Repositories;

public class PedidoRepositoryTests
{
    private readonly PedidoRepository _pedidoRepository;
    private readonly ApplicationDbContext _context;
    private readonly Mock<IDbConnection> _dbConnectionMock;
    private readonly Mock<IDapperWrapper> _dapperWrapperMock;

    public PedidoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _dbConnectionMock = new Mock<IDbConnection>();
        _dapperWrapperMock = new Mock<IDapperWrapper>();
        _pedidoRepository = new PedidoRepository(_context);
    }

    [Fact]
    public async Task FindByIdAsync_Should_ReturnPedido_When_PedidoExists()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new PedidoDb { Id = pedidoId, NumeroPedido = 123, ClienteId = Guid.NewGuid(), Status = "Recebido", ValorTotal = 100.0m, DataPedido = DateTime.UtcNow };
        await _context.Set<PedidoDb>().AddAsync(pedido);
        await _context.SaveChangesAsync();

        // Act
        var result = await _pedidoRepository.FindByIdAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pedidoId, result.Id);
    }

    [Fact]
    public async Task FindByIdAsync_Should_ReturnNull_When_PedidoDoesNotExist()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        // Act
        var result = await _pedidoRepository.FindByIdAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertAsync_Should_AddPedido()
    {
        // Arrange
        var pedido = new PedidoDb { Id = Guid.NewGuid(), NumeroPedido = 123, ClienteId = Guid.NewGuid(), Status = "Recebido", ValorTotal = 100.0m, DataPedido = DateTime.UtcNow };

        // Act
        await _pedidoRepository.InsertAsync(pedido, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Set<PedidoDb>().FindAsync(pedido.Id);
        Assert.NotNull(result);
        Assert.Equal(pedido.NumeroPedido, result.NumeroPedido);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdatePedido()
    {
        // Arrange
        var pedido = new PedidoDb { Id = Guid.NewGuid(), NumeroPedido = 123, ClienteId = Guid.NewGuid(), Status = "Recebido", ValorTotal = 100.0m, DataPedido = DateTime.UtcNow };
        await _context.Set<PedidoDb>().AddAsync(pedido);
        await _context.SaveChangesAsync();

        pedido.Status = "Pronto";

        // Act
        await _pedidoRepository.UpdateAsync(pedido, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Set<PedidoDb>().FindAsync(pedido.Id);
        Assert.NotNull(result);
        Assert.Equal("Pronto", result.Status);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemovePedido()
    {
        // Arrange
        var pedido = new PedidoDb { Id = Guid.NewGuid(), NumeroPedido = 123, ClienteId = Guid.NewGuid(), Status = "Recebido", ValorTotal = 100.0m, DataPedido = DateTime.UtcNow };
        await _context.Set<PedidoDb>().AddAsync(pedido);
        await _context.SaveChangesAsync();

        // Act
        await _pedidoRepository.DeleteAsync(pedido.Id, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Set<PedidoDb>().FindAsync(pedido.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task CommitAsync_Should_CommitTransaction()
    {
        // Arrange
        var pedido = new PedidoDb { Id = Guid.NewGuid(), NumeroPedido = 123, ClienteId = Guid.NewGuid(), Status = "Recebido", ValorTotal = 100.0m, DataPedido = DateTime.UtcNow };
        await _context.Set<PedidoDb>().AddAsync(pedido);

        // Act
        var result = await _context.CommitAsync(CancellationToken.None);

        // Assert
        Assert.True(result);
        var savedPedido = await _context.Set<PedidoDb>().FindAsync(pedido.Id);
        Assert.NotNull(savedPedido);
    }
}

public interface IDapperWrapper
{
    Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
}

public static class DapperMockExtensions
{
    public static void SetupDapperAsync<TMock, TResult>(this Mock<TMock> mock,
        Expression<Func<TMock, Task<TResult>>> expression, TResult result)
        where TMock : class, IDapperWrapper
    {
        mock.Setup(expression).ReturnsAsync(result);
    }
}