using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Api.Controllers.v2
{
    [FeatureGate(FeatureConstants.V2Feature)]
    [Route("api/v2/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Response from V2 API");
        }
    }
}
