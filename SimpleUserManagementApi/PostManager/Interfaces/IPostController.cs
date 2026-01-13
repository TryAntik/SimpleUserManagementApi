using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.PostManager.DTOs;

namespace SimpleUserManagementApi.PostManager.Interfaces;

public interface IPostController
{
    Task<ActionResult<List<PostDTO>>> GetAllPostsByUserIdAsync(Guid id, CancellationToken ct);
    Task<ActionResult<List<PostDTO>>> GetAllPostsAsync(CancellationToken ct);
    Task<ActionResult<PostDTO>> GetPostByIdAsync(Guid id, CancellationToken ct);
    Task<ActionResult> AddPostAsync(CreatePostDTO post, CancellationToken ct);
    Task<ActionResult> UpdatePostAsync(Guid id, UpdatePostDTO post, CancellationToken ct);
    Task<ActionResult> DeletePostAsync(Guid id, CancellationToken ct);
}