using Core.Infra.MessageBroker;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Worker.BackgroundServices;
using Worker.Dtos.Events;

namespace Worker.Tests.BackgroundServices;

public class PedidoStatusAlteradoBackgroundServiceTests
{
    private readonly Mock<ISqsService<PedidoStatusAlteradoEvent>> _sqsServiceMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<ILogger<PedidoStatusAlteradoBackgroundService>> _loggerMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;

    public PedidoStatusAlteradoBackgroundServiceTests()
    {
        _sqsServiceMock = new Mock<ISqsService<PedidoStatusAlteradoEvent>>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _loggerMock = new Mock<ILogger<PedidoStatusAlteradoBackgroundService>>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();

        _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);
        _serviceScopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IPedidoRepository))).Returns(_pedidoRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_Should_ProcessMessages()
    {
        // Arrange
        var pedidoStatusAlteradoEvent = new PedidoStatusAlteradoEvent
        {
            Id = Guid.NewGuid(),
            Status = "EmPreparacao"
        };
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(pedidoStatusAlteradoEvent);
        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PedidoDb { Id = pedidoStatusAlteradoEvent.Id, Status = "Recebido" });
        _pedidoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pedidoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var service = new PedidoStatusAlteradoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await service.StartAsync(cts.Token);

        // Assert
        _sqsServiceMock.Verify(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _pedidoRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_Should_LogError_When_ExceptionOccurs()
    {
        // Arrange
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        var service = new PedidoStatusAlteradoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await service.StartAsync(cts.Token);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_Should_NotUpdate_When_PedidoNotFound()
    {
        // Arrange
        var pedidoStatusAlteradoEvent = new PedidoStatusAlteradoEvent
        {
            Id = Guid.NewGuid(),
            Status = "EmPreparacao"
        };
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(pedidoStatusAlteradoEvent);
        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((PedidoDb)null);

        var service = new PedidoStatusAlteradoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await service.StartAsync(cts.Token);

        // Assert
        _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Never);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_Should_UpdateStatus_When_ValidTransition()
    {
        // Arrange
        var pedidoStatusAlteradoEvent = new PedidoStatusAlteradoEvent
        {
            Id = Guid.NewGuid(),
            Status = "EmPreparacao"
        };
        var pedidoExistente = new PedidoDb { Id = pedidoStatusAlteradoEvent.Id, Status = "Recebido" };
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(pedidoStatusAlteradoEvent);
        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(pedidoExistente);
        _pedidoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pedidoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var service = new PedidoStatusAlteradoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await service.StartAsync(cts.Token);

        // Assert
        _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal("EmPreparacao", pedidoExistente.Status);
    }
}
