using Moq;
using Moq.Protected;
using System.Net;

namespace SmokeTests;

public static class MockHttpMessageHandler
{
    public static Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpStatusCode statusCode, string content)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json"),
            })
            .Verifiable();

        return handlerMock;
    }
}