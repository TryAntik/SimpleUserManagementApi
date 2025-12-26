using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Auth.DTOs;

namespace SimpleUserManagementApi.Auth.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDTO request)
    {
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO request)
    {
        return Ok();
    }
}