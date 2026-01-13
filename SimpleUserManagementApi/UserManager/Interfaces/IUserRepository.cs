using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserRepository
{
    Task<bool> CheckUserExistsAsync(string email, string name);
    Task<bool> CheckUserExistsAsync(string email);
    Task<List<UserEntity>> GetAllUsersAsync(CancellationToken ct);
    Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken ct);
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken ct);
    Task AddUserAsync(UserEntity user);
    Task UpdateUserAsync(UserEntity updated);
    Task DeleteUserAsync(Guid userId);
}