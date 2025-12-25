using SimpleUserManagementApi.CRUD.DTOs;

namespace SimpleUserManagementApi.CRUD.Services;

public interface IPostService
{
    Task<List<PostDTO>> GetAllPostsAsync();
    Task<PostDTO?> GetPostByIdAsync(Guid id);
    Task<List<PostDTO>> GetAllPostsByUserIdAsync(Guid id);
    
    Task AddPostAsync(CreatePostDTO post);
    Task UpdatePostAsync(Guid id, UpdatePostDTO post);
    Task DeletePostAsync(Guid id);
}