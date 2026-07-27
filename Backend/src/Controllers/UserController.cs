using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoughBro.src.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProfile()
        {
            var firebaseUid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            return Ok(new { Message = "Authenticated!", UserId = firebaseUid });
        }
    }
}
