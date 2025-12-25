using SimpleUserManagementApi.CRUD.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SimpleUserManagementApi.CRUD.Controllers;

public interface IPostController
{
    Task<ActionResult<List<PostDTO>>> GetAllPostsByUserIdAsync(Guid id);
    Task<ActionResult<List<PostDTO>>> GetAllPostsAsync();
    Task<ActionResult<PostDTO>> GetPostByIdAsync(Guid id);
    Task<ActionResult> AddPostAsync(CreatePostDTO post);
    Task<ActionResult> UpdatePostAsync(Guid id, UpdatePostDTO post);
    Task<ActionResult> DeletePostAsync(Guid id);
}