using SimpleUserManagementApi.PostManager.DTOs;

namespace SimpleUserManagementApi.PostManager.Interfaces;

public interface IPostService
{
    Task<List<PostDTO>> GetAllPostsAsync(CancellationToken ct);
    Task<PostDTO?> GetPostByIdAsync(Guid id, CancellationToken ct);
    Task<List<PostDTO>> GetAllPostsByUserIdAsync(Guid id, CancellationToken ct);
    
    Task AddPostAsync(CreatePostDTO post, CancellationToken ct);
    Task UpdatePostAsync(Guid id, UpdatePostDTO post, CancellationToken ct);
    Task DeletePostAsync(Guid id, CancellationToken ct);
}