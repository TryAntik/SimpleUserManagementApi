using SimpleUserManagementApi.CRUD.DTOs;

namespace SimpleUserManagementApi.CRUD.Services;

public interface IPostService
{
    Task<List<PostDTO>> GetAllPostsAsync();
    Task<PostDTO?> GetPostByIdAsync(int id);
    Task<List<PostDTO>> GetAllPostsByUserIdAsync(int id);
    
    Task AddPostAsync(CreatePostDTO post);
    Task UpdatePostAsync(int id, UpdatePostDTO post);
    Task DeletePostAsync(int id);
}