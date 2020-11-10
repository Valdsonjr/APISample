using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace Api.V0.Controllers
{
    /// <summary>
    /// Saúde da API
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="healthCheckService"></param>
        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Obtém o status da API
        /// </summary>
        /// <remarks>Provém uma indicação da saúde da API</remarks>
        /// <response code="200">Relatório de saúde da API</response>
        [HttpGet]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status200OK)]
        public async Task<HealthReport> Get() => 
            await _healthCheckService.CheckHealthAsync();
    }
}
