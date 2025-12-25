using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Repositories;

public interface IPostRepository
{
    Task<List<PostEntity>> GetAllPostsAsync();
    Task<PostEntity?> GetPostByIdAsync(Guid postId);
    Task<List<PostEntity>?> GetAllPostsByUserIdAsync(Guid userId);
    Task AddPostAsync(PostEntity post);
    Task UpdatePostAsync(PostEntity updated);
    Task DeletePostAsync(Guid postId);
}   