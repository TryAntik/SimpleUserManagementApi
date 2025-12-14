using SimpleUserManagementApi.CRUD.DTOs;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Services;

public interface IUserService
{
    Task<List<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int id);
    
    Task AddUserAsync(CreateUserDTO user);
    Task UpdateUserAsync(int id, UpdateUserDTO updatedUser);
    Task DeleteUserAsync(int id);
}