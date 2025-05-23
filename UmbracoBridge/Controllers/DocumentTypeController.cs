using Microsoft.AspNetCore.Mvc;
using UmbracoBridge.Models;
using UmbracoBridge.Service;
using UmbracoBridge.Service.Interfaces;

namespace UmbracoBridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentTypeController : ControllerBase
    {
        private readonly ILogger<DocumentTypeController> _logger;
        private readonly IDocumentTypeService _umbracoService;

        public DocumentTypeController(ILogger<DocumentTypeController> logger, IDocumentTypeService umbracoBridgeService)
        {
            _logger = logger;
            _umbracoService = umbracoBridgeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ActionResult> PostDocumentType([FromBody] DocumentType documentType)
        {
            _logger.LogInformation("Create DocumentType...");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _umbracoService.PostDocumentType(documentType);

            if (result.IsSuccess)
            {
                Guid? createdId = result.Data;
                return Ok(createdId);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteDocumentType([FromRoute]Guid id)
        {
            _logger.LogInformation("Deleted DocumentType...");
            
            var result = await _umbracoService.DeleteDocumentType(id);

            if (result.IsSuccess)
            {
                return Ok();
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
