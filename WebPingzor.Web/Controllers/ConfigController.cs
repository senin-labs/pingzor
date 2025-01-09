using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebPingzor.Web.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetConfig()
        {
            Console.WriteLine("GetConfig called");

            // Secure logic
            return Ok(new
            {
                Success = false,
            });
        }
    }
}
