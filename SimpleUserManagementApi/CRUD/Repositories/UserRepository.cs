using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleUserManagementApi.CRUD.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<List<UserEntity>> GetAllUsersAsync()      
        => await _dbContext.Users.ToListAsync();
    
    public async Task<UserEntity?> GetUserByIdAsync(Guid id)
        => await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
    
    public async Task AddUserAsync(UserEntity user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UserEntity updatedUser)
    {
        _dbContext.Users.Update(updatedUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id);

        if (user == null)
            throw new NotFoundException($"user with id {id} not found");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}