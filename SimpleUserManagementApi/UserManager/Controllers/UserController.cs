using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.UserManager.Controllers;

[ApiController]
[Authorize(Policy = "AdminAccess")]
[Route("api/users")]
public class UserController : ControllerBase, IUserController
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserDTO> _createUserValidator;
    private readonly IValidator<UpdateUserDTO> _updateUserValidator;
    private readonly ILogger<UserController> _logger;
    
    public UserController(IUserService userService,
        ILogger<UserController> logger,
        IValidator<CreateUserDTO> createUserValidator,
        IValidator<UpdateUserDTO> updateUserValidator)
    {
        _userService = userService;
        _logger = logger;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsersAsync(CancellationToken ct)
        => Ok(await _userService.GetAllUsersAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetUserByIdAsync(Guid id, CancellationToken ct)
        => Ok(await _userService.GetUserByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult> AddUserAsync([FromBody] CreateUserDTO user, CancellationToken ct)
    {
        var result = await _createUserValidator.ValidateAsync(user, ct);
        if (!result.IsValid)
        {
            _logger.LogError("validation failed in AddUserAsync");
            return BadRequest(result.ToDictionary());
        }
        
        await _userService.AddUserAsync(user, ct);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserDTO user, CancellationToken ct)
    {
        var result = await _updateUserValidator.ValidateAsync(user, ct);
        if (!result.IsValid)
        {
            _logger.LogError("validation failed in UpdateUserAsync");
            return BadRequest(result.ToDictionary());
        }
        
        await _userService.UpdateUserAsync(id, user, ct);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUserAsync(Guid id, CancellationToken ct)
    {
        await _userService.DeleteUserAsync(id, ct);
        return Ok();
    }
}