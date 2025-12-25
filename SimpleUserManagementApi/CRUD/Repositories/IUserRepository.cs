using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> GetUserByIdAsync(Guid userId);
    Task AddUserAsync(UserEntity user);
    Task UpdateUserAsync(UserEntity updated);
    Task DeleteUserAsync(Guid userId);
}