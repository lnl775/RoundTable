using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UmbracoBridge.Controllers;
using UmbracoBridge.Models;
using UmbracoBridge.Service.Interfaces;

namespace RoundTable.Testing.API.Controller
{
    public class DocumentTypeControllerTests
    {
        private readonly Mock<ILogger<DocumentTypeController>> _loggerMock;
        private readonly Mock<IDocumentTypeService> _documentTypeServiceMock;
        private readonly DocumentTypeController _controller;

        public DocumentTypeControllerTests()
        {
            _loggerMock = new Mock<ILogger<DocumentTypeController>>();
            _documentTypeServiceMock = new Mock<IDocumentTypeService>();
            _controller = new DocumentTypeController(_loggerMock.Object, _documentTypeServiceMock.Object);
        }

        [Fact]
        public async Task PostDocumentType_ValidDocumentType_ReturnsCreatedGuid()
        {
            // Arrange
            var documentType = new DocumentType();
            var expectedGuid = Guid.NewGuid();
            _documentTypeServiceMock
                .Setup(service => service.PostDocumentType(documentType))
                .ReturnsAsync(new ApiResponse<Guid>
                {
                    IsSuccess = true,
                    Data = expectedGuid
                });

            // Act
            var result = await _controller.PostDocumentType(documentType);

            // Assert
            var okResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(expectedGuid, okResult.Value);
        }

        [Fact]
        public async Task PostDocumentType_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var documentType = new DocumentType();
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.PostDocumentType(documentType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);

            var errors = serializableError["Error"];
            Assert.Single((System.Collections.IEnumerable)errors);
            Assert.Equal("Invalid model", ((string[])errors)[0]);
        }

        [Fact]
        public async Task PostDocumentType_ServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var documentType = new DocumentType();
            var errorMessage = "Error creating document type";
            _documentTypeServiceMock
                .Setup(service => service.PostDocumentType(documentType))
                .ReturnsAsync(new ApiResponse<Guid>
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage
                });

            // Act
            var result = await _controller.PostDocumentType(documentType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteDocumentType_ValidId_ReturnsOk()
        {
            // Arrange
            var documentTypeId = Guid.NewGuid();
            _documentTypeServiceMock
                .Setup(service => service.DeleteDocumentType(documentTypeId))
                .ReturnsAsync(new ApiResponse<bool>
                {
                    IsSuccess = true
                });

            // Act
            var result = await _controller.DeleteDocumentType(documentTypeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDocumentType_ServiceReturnsError_ReturnsNotFound()
        {
            // Arrange
            var documentTypeId = Guid.NewGuid();
            var errorMessage = "Document type not found";
            _documentTypeServiceMock
                .Setup(service => service.DeleteDocumentType(documentTypeId))
                .ReturnsAsync(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage
                });

            // Act
            var result = await _controller.DeleteDocumentType(documentTypeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(errorMessage, notFoundResult.Value);
        }
    }
}
