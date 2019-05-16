using Microsoft.AspNetCore.Mvc;

namespace PensionsRegulatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet()]
        public ActionResult Get()
        {
            return StatusCode(200);
        }
    }
}
