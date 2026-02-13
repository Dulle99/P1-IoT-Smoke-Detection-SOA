using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthControler : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Gateway Service is healthy");
        }
    }
}
