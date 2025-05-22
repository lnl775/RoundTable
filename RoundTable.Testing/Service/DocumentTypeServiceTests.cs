using Moq;
using Moq.Protected;
using System.Net;
using UmbracoBridge.Models;
using UmbracoBridge.Service;

namespace UmbracoBridge.Tests
{
    public class DocumentTypeServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly DocumentTypeService _documentTypeService;

        public DocumentTypeServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            _httpClientFactoryMock.Setup(_ => _.CreateClient("umbraco")).Returns(httpClient);
            _documentTypeService = new DocumentTypeService(_httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task PostDocumentType_Success_ReturnsCreatedGuid()
        {
            // Arrange
            var documentType = new DocumentType();
            var expectedGuid = Guid.NewGuid();
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created,
                    Headers = { { "Umb-Generated-Resource", expectedGuid.ToString() } },
                });

            // Act
            var result = await _documentTypeService.PostDocumentType(documentType);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedGuid, result.Data);
        }

        [Fact]
        public async Task PostDocumentType_Failure_ReturnsErrorMessage()
        {
            // Arrange
            var documentType = new DocumentType();
            var errorMessage = "Error 400: Bad Request";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(errorMessage),
                });

            // Act
            var result = await _documentTypeService.PostDocumentType(documentType);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error 400: Bad Request", result.ErrorMessage);
        }

        [Fact]
        public async Task PostDocumentType_Exception_ReturnsErrorMessage()
        {
            // Arrange
            var documentType = new DocumentType();
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new Exception("Network error"));

            // Act
            var result = await _documentTypeService.PostDocumentType(documentType);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("An error occurred: Network error", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteDocumentType_Success_ReturnsTrue()
        {
            // Arrange
            var documentTypeId = Guid.NewGuid();
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent, // 204 No Content indicates success
                });

            // Act
            var result = await _documentTypeService.DeleteDocumentType(documentTypeId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteDocumentType_Failure_ReturnsErrorMessage()
        {
            // Arrange
            var documentTypeId = Guid.NewGuid();
            var errorMessage = "Not Found";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(errorMessage),
                });

            // Act
            var result = await _documentTypeService.DeleteDocumentType(documentTypeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error 404: Not Found", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteDocumentType_Exception_ReturnsErrorMessage()
        {
            // Arrange
            var documentTypeId = Guid.NewGuid();
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new Exception("Network error"));

            // Act
            var result = await _documentTypeService.DeleteDocumentType(documentTypeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("An error occurred: Network error", result.ErrorMessage);
        }
    }
}