using Core.Infra.MessageBroker;
using Domain.Entities;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.BackgroundServices;
using Worker.Dtos.Events;

namespace Worker.Tests.BackgroundServices;

public class ProdutoExcluidoBackgroundServiceTests
{
    private readonly Mock<ISqsService<ProdutoExcluidoEvent>> _sqsServiceMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<ILogger<ProdutoExcluidoBackgroundService>> _loggerMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;

    public ProdutoExcluidoBackgroundServiceTests()
    {
        _sqsServiceMock = new Mock<ISqsService<ProdutoExcluidoEvent>>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _loggerMock = new Mock<ILogger<ProdutoExcluidoBackgroundService>>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _produtoRepositoryMock = new Mock<IProdutoRepository>();

        _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);
        _serviceScopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IProdutoRepository))).Returns(_produtoRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_Should_ProcessMessages()
    {
        // Arrange
        var produtoExcluidoEvent = new ProdutoExcluidoEvent { Id = Guid.NewGuid() };
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(produtoExcluidoEvent);
        _produtoRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProdutoDb { Id = produtoExcluidoEvent.Id, Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 100.0m, Categoria = "Categoria Teste", Ativo = true });
        _produtoRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var service = new ProdutoExcluidoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await service.StartAsync(cts.Token);

        // Assert
        _sqsServiceMock.Verify(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _produtoRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _produtoRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_Should_LogError_When_ExceptionOccurs()
    {
        // Arrange
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        var service = new ProdutoExcluidoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

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
    public async Task ExecuteAsync_Should_NotDelete_When_ProdutoNotFound()
    {
        // Arrange
        var produtoExcluidoEvent = new ProdutoExcluidoEvent { Id = Guid.NewGuid() };
        _sqsServiceMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(produtoExcluidoEvent);
        _produtoRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((ProdutoDb)null);

        var service = new ProdutoExcluidoBackgroundService(_sqsServiceMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await service.StartAsync(cts.Token);

        // Assert
        _produtoRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
