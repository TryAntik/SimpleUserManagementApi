using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.PostManager.Interfaces;

public interface IPostRepository
{
    Task<List<PostEntity>> GetAllPostsAsync(CancellationToken ct);
    Task<PostEntity?> GetPostByIdAsync(Guid postId, CancellationToken ct);
    Task<List<PostEntity>?> GetAllPostsByUserIdAsync(Guid userId, CancellationToken ct);
    Task AddPostAsync(PostEntity post, CancellationToken ct);
    Task UpdatePostAsync(PostEntity updated, CancellationToken ct);
    Task DeletePostAsync(Guid postId, CancellationToken ct);
}   