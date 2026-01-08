using Microsoft.AspNetCore.Authorization;
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

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request) 
        => Ok(await _userService.LoginUserAsync(request));

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDTO requestDto)
    {
        await _userService.RegisterUserAsync(requestDto); 
        return Ok();
    }
}