using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.CRUD.DTOs;
using SimpleUserManagementApi.CRUD.Services;

namespace SimpleUserManagementApi.CRUD.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase, IUserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
        => _userService = userService;

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsersAsync()
        => Ok(await _userService.GetAllUsersAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int id)
        => Ok(await _userService.GetUserByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> AddUserAsync([FromBody] CreateUserDTO user)
    {
        await _userService.AddUserAsync(user);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserDTO user)
    {
        await _userService.UpdateUserAsync(id, user);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok();
    }
}