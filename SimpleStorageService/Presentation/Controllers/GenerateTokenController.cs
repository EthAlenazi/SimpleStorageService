using Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MySecureController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public MySecureController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetSecureData()
        {
            var token = GenerateJwtTokenService.GenerateJwtToken(_configuration);
            return Ok(new { message = $"This is secure data : {token}" });
        }
    }
}
