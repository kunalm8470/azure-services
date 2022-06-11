using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AzureAppConfigurationDto _config;
        public ValuesController(IOptionsSnapshot<AzureAppConfigurationDto> snapshotOptionsAccessor)
        {
            _config = snapshotOptionsAccessor.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Key1 = _config.Key1,
                Key2 = _config.Key2,
                Key3 = _config.Key3
            });
        }
    }
}
