using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.UserManager.DTOs;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserController
{
    Task<ActionResult<List<UserDTO>>> GetAllUsersAsync(CancellationToken ct);
    Task<ActionResult<UserDTO>> GetUserByIdAsync(Guid id, CancellationToken ct);
    Task<ActionResult> AddUserAsync(CreateUserDTO user, CancellationToken ct);
    Task<ActionResult> UpdateUserAsync(Guid id, UpdateUserDTO user, CancellationToken ct);
    Task<ActionResult> DeleteUserAsync(Guid id, CancellationToken ct);
}