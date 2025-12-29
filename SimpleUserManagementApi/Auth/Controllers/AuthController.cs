using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.Auth.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthController(IUserService userService)
        => _userService = userService;

    /*
     [HttpPost("register")]
     public async Task<ActionResult> Register([FromBody] RegisterDTO request)
     {
        
     }*/
    
       [HttpPost("login")]
       public async Task<ActionResult<string>> Login([FromBody] LoginDTO request)
       {
           await _userService.LoginUserAsync(request);
           return Ok();
       }
}