using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> GetUserByIdAsync(int userId);
    Task AddUserAsync(UserEntity user);
    Task UpdateUserAsync(UserEntity updated);
    Task DeleteUserAsync(int id);
}