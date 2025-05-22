using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using UmbracoBridge.Models;
using UmbracoBridge.Service;

namespace RoundTable.Testing.Service
{
    public class HealthServiceTests
    {
        private readonly Mock<ILogger<HealthService>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HealthService _healthService;

        public HealthServiceTests()
        {
            _loggerMock = new Mock<ILogger<HealthService>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(_ => _.CreateClient("umbraco")).Returns(httpClient);
            _healthService = new HealthService(_loggerMock.Object, _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task CheckHealthAsync_Success_ReturnsHealthyResult()
        {
            // Arrange
            var healthCheckGroup = new HealthCheckGroup
            {
                Total = 1,
                Items = null
            };
            var jsonResponse = JsonSerializer.Serialize(healthCheckGroup);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });

            // Act
            var result = await _healthService.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.Equal((int)HealthStatus.Healthy, (int)result.Status);
            Assert.Contains("Total", result.Data);
            Assert.Contains("Items", result.Data);
        }

        [Fact]
        public async Task CheckHealthAsync_Failure_ReturnsUnhealthyResult()
        {
            // Arrange
            var errorMessage = "Service Unavailable";
            var jsonResponse = $"{{\"error\":\"{errorMessage}\"}}";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    Content = new StringContent(jsonResponse),
                });

            // Act
            var result = await _healthService.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.Equal((int)HealthStatus.Unhealthy, (int)result.Status);
            Assert.Contains($"Service Unavailable", result.Description);
        }

        [Fact]
        public async Task HealthCheckGroup_Exception_ReturnsErrorMessage()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new Exception("Network error"));

            // Act
            var result = await _healthService.HealthCheckGroup();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("An error occurred: Network error", result.ErrorMessage);
        }
    }
}
