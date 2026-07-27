using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TodoApp.API.Controllers;

[ApiController]
[Route("api/controller")]
public class BaseApiController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
