using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.UserManager.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Logger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext dbContext, Logger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> CheckUserExistsAsync(string email, string name, CancellationToken ct = default)
        => await _dbContext.Users.AnyAsync(u =>
            u.Email == email.ToLowerInvariant().Trim()
            && u.Name == name.Trim());
    
    public async Task<bool> CheckUserExistsAsync(string email, CancellationToken ct = default)
        => await _dbContext.Users.AnyAsync(u => 
            u.Email == email.ToLowerInvariant().Trim(), ct);
    
    public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken ct = default)
        => await _dbContext.Users.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower(), ct);
    
    public async Task<List<UserEntity>> GetAllUsersAsync(CancellationToken ct = default)      
        => await _dbContext.Users.ToListAsync(ct);
    
    public async Task<UserEntity?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
        => await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id, ct);
    
    public async Task AddUserAsync(UserEntity user, CancellationToken ct = default)
    { 
        _dbContext.Users.Add(user);
        _logger.LogInformation("User created: {@User}", user);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateUserAsync(UserEntity updatedUser, CancellationToken ct = default)
    {
        _dbContext.Users.Update(updatedUser);
        _logger.LogInformation("User updated: {@UpdatedUser}", updatedUser);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id, ct);

        if (user == null)
        {
            _logger.LogWarning("User not found: {@User}", user);
            throw new NotFoundException($"user with id {id} not found");
        }

        _dbContext.Users.Remove(user);
        _logger.LogInformation("User removed from db: {@User}", user);
        await _dbContext.SaveChangesAsync(ct);
    }
}