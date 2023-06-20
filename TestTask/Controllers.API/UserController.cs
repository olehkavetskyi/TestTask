using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("current-user")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }
}
