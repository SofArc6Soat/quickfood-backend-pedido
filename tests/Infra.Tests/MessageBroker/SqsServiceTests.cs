using Amazon.SQS;
using Amazon.SQS.Model;
using Core.Infra.MessageBroker;
using Moq;
using System.Text.Json;

namespace Infra.Tests.MessageBroker;

public class SqsServiceTests
{
    private readonly Mock<IAmazonSQS> _sqsClientMock;
    private readonly SqsService<TestMessage> _sqsService;
    private readonly string _queueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue";

    public SqsServiceTests()
    {
        _sqsClientMock = new Mock<IAmazonSQS>();
        _sqsService = new SqsService<TestMessage>(_sqsClientMock.Object, _queueUrl);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnTrue_WhenMessageIsSentSuccessfully()
    {
        // Arrange
        var testMessage = new TestMessage { Content = "Test content" };
        _sqsClientMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SendMessageResponse { MessageId = "12345" });

        // Act
        var result = await _sqsService.SendMessageAsync(testMessage);

        // Assert
        Assert.True(result);
        _sqsClientMock.Verify(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnFalse_WhenMessageIsNotSent()
    {
        // Arrange
        var testMessage = new TestMessage { Content = "Test content" };
        _sqsClientMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SendMessageResponse)null);

        // Act
        var result = await _sqsService.SendMessageAsync(testMessage);

        // Assert
        Assert.False(result);
        _sqsClientMock.Verify(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReceiveMessagesAsync_ShouldReturnMessage_WhenMessageIsReceived()
    {
        // Arrange
        var testMessage = new TestMessage { Content = "Test content" };
        var messageBody = JsonSerializer.Serialize(testMessage);
        _sqsClientMock.Setup(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReceiveMessageResponse
            {
                Messages = new List<Message>
                {
                        new Message { Body = messageBody, ReceiptHandle = "receipt-handle" }
                }
            });
        _sqsClientMock.Setup(x => x.DeleteMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteMessageResponse());

        // Act
        var result = await _sqsService.ReceiveMessagesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testMessage.Content, result.Content);
        _sqsClientMock.Verify(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _sqsClientMock.Verify(x => x.DeleteMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReceiveMessagesAsync_ShouldReturnNull_WhenNoMessagesAreReceived()
    {
        // Arrange
        _sqsClientMock.Setup(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReceiveMessageResponse { Messages = new List<Message>() });

        // Act
        var result = await _sqsService.ReceiveMessagesAsync(CancellationToken.None);

        // Assert
        Assert.Null(result);
        _sqsClientMock.Verify(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private class TestMessage
    {
        public string Content { get; set; }
    }
}