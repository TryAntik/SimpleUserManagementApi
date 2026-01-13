using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.Auth.Interfaces;
using SimpleUserManagementApi.Auth.RefreshToken;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.Auth.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
        => _userService = userService;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request, CancellationToken ct) 
        => Ok(await _userService.LoginUserAsync(request, ct));

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDTO requestDto)
    {
        await _userService.RegisterUserAsync(requestDto); 
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshResponseDTO>> RefreshToken([FromBody] RefreshRequestDTO request, CancellationToken ct)
    {
        try
        {
            var response = await _userService.RefreshTokenAsync(request, ct);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("refresh token is invalid");
        }
        catch (NotFoundException)
        {
            return NotFound("user was not found");
        }
    }
}
