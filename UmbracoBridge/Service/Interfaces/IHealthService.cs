using UmbracoBridge.Models;

namespace UmbracoBridge.Service.Interfaces
{
    public interface IHealthService
    {
        Task<ApiResponse<HealthCheckGroup>> HealthCheckGroup();
    }
}
