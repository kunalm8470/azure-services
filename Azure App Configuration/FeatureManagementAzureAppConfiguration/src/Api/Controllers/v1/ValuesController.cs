using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IFeatureManagerSnapshot _featureManager;

        public ValuesController(IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (await _featureManager.IsEnabledAsync(FeatureConstants.BetaV1Feature))
            {
                return Ok("Response from Beta API");
            }
            else
            {
                return Ok("Response");
            }
        }
    }
}
