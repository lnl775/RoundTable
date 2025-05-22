using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UmbracoBridge.Controllers;
using UmbracoBridge.Models;
using UmbracoBridge.Service.Interfaces;

namespace RoundTable.Testing.API.Controller
{
    public class HealthCheckControllerTests
    {
        private readonly Mock<ILogger<HealthCheckController>> _loggerMock;
        private readonly Mock<IHealthService> _healthCheckServiceMock;
        private readonly HealthCheckController _controller;

        public HealthCheckControllerTests()
        {
            _loggerMock = new Mock<ILogger<HealthCheckController>>();
            _healthCheckServiceMock = new Mock<IHealthService>();
            _controller = new HealthCheckController(_loggerMock.Object, _healthCheckServiceMock.Object);
        }

        [Fact]
        public async Task HealthCheck_Success_ReturnsOkWithHealthCheckGroup()
        {
            // Arrange
            var healthCheckGroup = new HealthCheckGroup(); // Populate with necessary data
            _healthCheckServiceMock
                .Setup(service => service.HealthCheckGroup())
                .ReturnsAsync(new ApiResponse<HealthCheckGroup>
                {
                    IsSuccess = true,
                    Data = healthCheckGroup
                });

            // Act
            var result = await _controller.HealthCheck();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(healthCheckGroup, okResult.Value);
        }

        [Fact]
        public async Task HealthCheck_Failure_ReturnsBadRequest()
        {
            // Arrange
            var errorMessage = "Health check failed";
            _healthCheckServiceMock
                .Setup(service => service.HealthCheckGroup())
                .ReturnsAsync(new ApiResponse<HealthCheckGroup>
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage
                });

            // Act
            var response = await _controller.HealthCheck();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }
    }
}
