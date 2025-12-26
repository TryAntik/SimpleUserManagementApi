using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.UserManager.DTOs;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserController
{
    Task<ActionResult<List<UserDTO>>> GetAllUsersAsync();
    Task<ActionResult<UserDTO>> GetUserByIdAsync(Guid id);
    Task<ActionResult> AddUserAsync(CreateUserDTO user);
    Task<ActionResult> UpdateUserAsync(Guid id, UpdateUserDTO user);
    Task<ActionResult> DeleteUserAsync(Guid id);
}