using FluentValidation;
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
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IValidator<RegisterRequestDTO> _registerValidator;
    private readonly IValidator<LoginRequestDTO> _loginValidator;

    public AuthController(IUserService userService, 
        IRefreshTokenService refreshTokenService,
        IValidator<RegisterRequestDTO> registerValidator,
        IValidator<LoginRequestDTO> loginRequest)
    {
        _userService = userService; 
        _refreshTokenService = refreshTokenService;
        _registerValidator = registerValidator;
        _loginValidator = loginRequest;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request, CancellationToken ct)
    {
        var result = await _loginValidator.ValidateAsync(request, ct);
        if (!result.IsValid) return BadRequest(result.ToDictionary());

        return Ok(await _userService.LoginUserAsync(request, ct));
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDTO requestDTO, CancellationToken ct)
    {
        var result = await _registerValidator.ValidateAsync(requestDTO, ct);
        if (!result.IsValid) return BadRequest(result.ToDictionary());
        
        await _userService.RegisterUserAsync(requestDTO, ct);
        return Ok();
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout(LogoutRequestDTO request, CancellationToken ct)
    {
        await _refreshTokenService.RevokeTokenAsync(request.tokenId, ct);
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
