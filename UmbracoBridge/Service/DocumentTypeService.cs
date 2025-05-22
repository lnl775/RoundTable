using UmbracoBridge.Models;
using UmbracoBridge.Service.Interfaces;

namespace UmbracoBridge.Service
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IHttpClientFactory _factory;

        public DocumentTypeService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<ApiResponse<Guid>> PostDocumentType(DocumentType documentType)
        {
            try
            {
                using var _httpClient = _factory.CreateClient("umbraco");
                var response = await _httpClient.PostAsJsonAsync("document-type", documentType);

                if (response.IsSuccessStatusCode)
                {
                    var resource = response.Headers.GetValues("Umb-Generated-Resource").SingleOrDefault();
                    Guid.TryParse(resource, out var created);
                    return new ApiResponse<Guid>
                    {
                        IsSuccess = true,
                        Data = created
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<Guid>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error {(int)response.StatusCode}: {errorMessage}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Guid>
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
        public async Task<ApiResponse<bool>> DeleteDocumentType(Guid documentTypeId)
        {
            try
            {
                using var _httpClient = _factory.CreateClient("umbraco");
                var response = await _httpClient.DeleteAsync($"document-type/{documentTypeId}");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>
                    {
                        IsSuccess = true,
                        Data = true
                    };
                }
                else 
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error {(int)response.StatusCode}: {errorMessage}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
