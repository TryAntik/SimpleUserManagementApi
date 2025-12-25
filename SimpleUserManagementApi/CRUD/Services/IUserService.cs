using SimpleUserManagementApi.CRUD.DTOs;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Services;

public interface IUserService
{
    Task<List<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(Guid id);
    
    Task AddUserAsync(CreateUserDTO user);
    Task UpdateUserAsync(Guid id, UpdateUserDTO updatedUser);
    Task DeleteUserAsync(Guid id);
}