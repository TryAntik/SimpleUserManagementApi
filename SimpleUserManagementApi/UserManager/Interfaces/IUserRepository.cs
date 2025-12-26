using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserRepository
{
    Task<bool> CheckUserExistsAsync(string email, string name);
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> GetUserByIdAsync(Guid userId);
    Task AddUserAsync(UserEntity user);
    Task UpdateUserAsync(UserEntity updated);
    Task DeleteUserAsync(Guid userId);
}