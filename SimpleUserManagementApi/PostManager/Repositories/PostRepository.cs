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

    public async Task<List<PostEntity>> GetAllPostsAsync(CancellationToken ct = default)
        => await _dbContext.Posts.ToListAsync(ct);

    public async Task<PostEntity?> GetPostByIdAsync(Guid postId, CancellationToken ct = default)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(a => a.Id == postId, ct);
        return post;
    }

    public async Task<List<PostEntity>?> GetAllPostsByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await _dbContext.Posts
            .Where(p => p.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task AddPostAsync(PostEntity post, CancellationToken ct = default)
    {
        await _dbContext.Posts.AddAsync(post, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdatePostAsync(PostEntity updated, CancellationToken ct = default)
    {
        _dbContext.Posts.Update(updated);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeletePostAsync(Guid id, CancellationToken ct = default)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(a => a.Id == id, ct);
        
        if(post == null)
            throw new NotFoundException($"post with id {id} not found");
        
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync(ct);
    }
}