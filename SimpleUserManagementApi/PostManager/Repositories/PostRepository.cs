using Microsoft.EntityFrameworkCore;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.Models;
using SimpleUserManagementApi.PostManager.Interfaces;

namespace SimpleUserManagementApi.PostManager.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostRepository(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<List<PostEntity>> GetAllPostsAsync()
        => await _dbContext.Posts.ToListAsync();

    public async Task<PostEntity?> GetPostByIdAsync(Guid postId)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(a => a.Id == postId);
        return post;
    }

    public async Task<List<PostEntity>?> GetAllPostsByUserIdAsync(Guid userId)
    {
        return await _dbContext.Posts
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task AddPostAsync(PostEntity post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePostAsync(PostEntity updated)
    {
        _dbContext.Posts.Update(updated);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePostAsync(Guid id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(a => a.Id == id);
        
        if(post == null)
            throw new NotFoundException($"post with id {id} not found");
        
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }
}