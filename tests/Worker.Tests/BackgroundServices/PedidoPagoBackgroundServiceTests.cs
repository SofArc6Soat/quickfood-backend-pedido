using Core.Infra.MessageBroker;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Worker.BackgroundServices;
using Worker.Dtos.Events;

namespace Worker.Tests.BackgroundServices
{
    public class PedidoPagoBackgroundServiceTests
    {
        private readonly Mock<ISqsService<PedidoPagoEvent>> _sqsClientMock;
        private readonly Mock<ISqsService<PedidoRecebidoEvent>> _sqsPedidoRecebidoMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IServiceScope> _serviceScopeMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<ILogger<PedidoPagoBackgroundService>> _loggerMock;
        private readonly PedidoPagoBackgroundService _backgroundService;

        public PedidoPagoBackgroundServiceTests()
        {
            _sqsClientMock = new Mock<ISqsService<PedidoPagoEvent>>();
            _sqsPedidoRecebidoMock = new Mock<ISqsService<PedidoRecebidoEvent>>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _loggerMock = new Mock<ILogger<PedidoPagoBackgroundService>>();

            _serviceScopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IPedidoRepository))).Returns(_pedidoRepositoryMock.Object);
            _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);

            _backgroundService = new PedidoPagoBackgroundService(_sqsClientMock.Object, _sqsPedidoRecebidoMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_LogError_When_ExceptionOccurs()
        {
            // Arrange
            var stoppingTokenSource = new CancellationTokenSource();
            var stoppingToken = stoppingTokenSource.Token;

            _sqsClientMock.Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var executeTask = _backgroundService.StartAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(2));
            stoppingTokenSource.Cancel();
            await executeTask;

            // Assert
            _loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ProcessMessageAsync_Should_Not_Process_When_MessageIsNull()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            // Act
            await _backgroundService.ProcessMessageAsync(null, cancellationToken);

            // Assert
            _pedidoRepositoryMock.Verify(x => x.Query(), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _sqsPedidoRecebidoMock.Verify(x => x.SendMessageAsync(It.IsAny<PedidoRecebidoEvent>()), Times.Never);
        }

        [Fact]
        public async Task ProcessMessageAsync_Should_Not_Process_When_StatusIsNotPago()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var message = new PedidoPagoEvent { PedidoId = Guid.NewGuid(), Status = "Pendente" };

            // Act
            await _backgroundService.ProcessMessageAsync(message, cancellationToken);

            // Assert
            _pedidoRepositoryMock.Verify(x => x.Query(), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _sqsPedidoRecebidoMock.Verify(x => x.SendMessageAsync(It.IsAny<PedidoRecebidoEvent>()), Times.Never);
        }
    }
}