using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using UmbracoBridge.Models;
using UmbracoBridge.Service.Interfaces;

namespace UmbracoBridge.Service
{
    public class HealthService : IHealthCheck, IHealthService
    {
        private readonly ILogger<HealthService> _logger;
        private readonly IHttpClientFactory _factory;

        public HealthService(ILogger<HealthService> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Healthcheck...");
            var result = await HealthCheckGroup();
            if (result.IsSuccess)
            {
                var collection = new Dictionary<string, object>();
                collection.Add(nameof(result.Data.Total), result.Data.Total);
                collection.Add(nameof(result.Data.Items), result.Data.Items);

                return HealthCheckResult.Healthy(nameof(HealthCheckResult.Healthy).ToString(), collection);
            }
            return HealthCheckResult.Unhealthy(result.ErrorMessage);
        }

        public async Task<ApiResponse<HealthCheckGroup>> HealthCheckGroup()
        {
            try
            {
                using var _httpClient = _factory.CreateClient("umbraco");
                var response = await _httpClient.GetAsync("health-check-group?skip=0&take=100");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<HealthCheckGroup>(responseBody);
                    return new ApiResponse<HealthCheckGroup>
                    {
                        IsSuccess = true,
                        Data = result
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<HealthCheckGroup>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error {response.StatusCode}: {errorMessage}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<HealthCheckGroup>
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
