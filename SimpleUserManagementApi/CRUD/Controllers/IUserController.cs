using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.CRUD.DTOs;
    
namespace SimpleUserManagementApi.CRUD.Controllers;

public interface IUserController
{
    Task<ActionResult<List<UserDTO>>> GetAllUsersAsync();
    Task<ActionResult<UserDTO>> GetUserByIdAsync(int id);
    Task<ActionResult> AddUserAsync(CreateUserDTO user);
    Task<ActionResult> UpdateUserAsync(int id, UpdateUserDTO user);
    Task<ActionResult> DeleteUserAsync(int id);
}