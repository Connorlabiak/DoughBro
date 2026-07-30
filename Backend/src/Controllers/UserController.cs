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
            string? firebaseUid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
            if (firebaseUid is null)
            {
                return Unauthorized("User ID not found in claims.");
            }

            return Ok(new { Message = "Authenticated!", UserId = firebaseUid });
        }
    }
}
