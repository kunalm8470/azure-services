using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly JsonTypicodeService _service;

        public ValuesController(
            ILogger<ValuesController> logger,
            JsonTypicodeService service
        )
        {
            _logger = logger;
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("divide/{num1:double}/{num2:double}")]
        public double Divide(double num1, double num2)
        {
            if (num2 == 0)
            {
                throw new ArgumentException("Denominator cannot be zero", nameof(num2));
            }

            return num1 / num2;
        }

        [HttpGet("user/{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id, CancellationToken token)
        {
            User? found = await _service.GetUserAsync(id, token);

            if (found == default)
            {
                return NotFound(new { message = $"User not found with id {id}" });
            }

            return Ok(found);
        }
    }
}
