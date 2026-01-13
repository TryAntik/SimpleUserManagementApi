using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserRepository
{
    Task<bool> CheckUserExistsAsync(string email, string name, CancellationToken ct);
    Task<bool> CheckUserExistsAsync(string email, CancellationToken ct);
    Task<List<UserEntity>> GetAllUsersAsync(CancellationToken ct);
    Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken ct);
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken ct);
    Task AddUserAsync(UserEntity user, CancellationToken ct);
    Task UpdateUserAsync(UserEntity updated, CancellationToken ct);
    Task DeleteUserAsync(Guid userId, CancellationToken ct);
}