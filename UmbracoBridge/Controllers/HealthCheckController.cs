using Microsoft.AspNetCore.Mvc;
using UmbracoBridge.Models;
using UmbracoBridge.Service.Interfaces;

namespace UmbracoBridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : Controller
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IHealthService _healthCheckService;

        public HealthCheckController(ILogger<HealthCheckController> logger, IHealthService healthCheckService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
        }

        [HttpGet(Name = "Healthcheck")]
        [ProducesResponseType(typeof(HealthCheckGroup), StatusCodes.Status200OK)]
        public async Task<ActionResult<HealthCheckGroup>> HealthCheck()
        {
            _logger.LogInformation("Healthcheck...");
            var result = await _healthCheckService.HealthCheckGroup();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
