using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterDTO request);
    Task<UserEntity> LoginUserAsync(LoginDTO request);
    Task<List<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(Guid id);
    Task AddUserAsync(CreateUserDTO user);
    Task UpdateUserAsync(Guid id, UpdateUserDTO updatedUser);
    Task DeleteUserAsync(Guid id);
}