using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(IConfiguration configuration, ILogger<ValuesController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string secret = _configuration.GetValue<string>("Secret");
            _logger.LogInformation("Secret from Azure Key Vault: {0}", secret);
            return Ok(secret);
        }
    }
}
