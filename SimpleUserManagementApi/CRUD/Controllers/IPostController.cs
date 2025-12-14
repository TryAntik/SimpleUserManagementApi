using SimpleUserManagementApi.CRUD.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SimpleUserManagementApi.CRUD.Controllers;

public interface IPostController
{
    Task<ActionResult<List<PostDTO>>> GetAllPostsByUserIdAsync(int id);
    Task<ActionResult<List<PostDTO>>> GetAllPostsAsync();
    Task<ActionResult<PostDTO>> GetPostByIdAsync(int id);
    Task<ActionResult> AddPostAsync(CreatePostDTO post);
    Task<ActionResult> UpdatePostAsync(int id, UpdatePostDTO post);
    Task<ActionResult> DeletePostAsync(int id);
}