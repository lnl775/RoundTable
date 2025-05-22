using UmbracoBridge.Models;

namespace UmbracoBridge.Service.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<ApiResponse<Guid>> PostDocumentType(DocumentType documentType);
        Task<ApiResponse<bool>> DeleteDocumentType(Guid documentTypeId);
    }
}
