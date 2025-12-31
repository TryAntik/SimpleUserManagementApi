using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.UserManager.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UserController : ControllerBase, IUserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService) 
        => _userService = userService;

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsersAsync()
        => Ok(await _userService.GetAllUsersAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetUserByIdAsync(Guid id)
        => Ok(await _userService.GetUserByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> AddUserAsync([FromBody] CreateUserDTO user)
    {
        await _userService.AddUserAsync(user);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserDTO user)
    {
        await _userService.UpdateUserAsync(id, user);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUserAsync(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok();
    }
}