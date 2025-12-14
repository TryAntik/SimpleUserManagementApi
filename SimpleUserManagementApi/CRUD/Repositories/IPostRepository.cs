using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Repositories;

public interface IPostRepository
{
    Task<List<PostEntity>> GetAllPostsAsync();
    Task<PostEntity?> GetPostByIdAsync(int postId);
    Task<List<PostEntity>?> GetAllPostsByUserIdAsync(int userId);
    Task AddPostAsync(PostEntity post);
    Task UpdatePostAsync(PostEntity updated);
    Task DeletePostAsync(int id);
}   