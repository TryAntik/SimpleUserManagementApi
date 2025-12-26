using Microsoft.EntityFrameworkCore;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.UserManager.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<bool> CheckUserExistsAsync(string email, string name)
        => await _dbContext.Users.AnyAsync(
            a => a.Email.ToLower() == email.ToLower() || 
                 a.Name.ToLower() == name.ToLower());
    
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