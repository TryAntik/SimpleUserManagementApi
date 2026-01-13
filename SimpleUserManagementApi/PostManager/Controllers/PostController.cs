using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SimpleUserManagementApi.PostManager.DTOs;
using SimpleUserManagementApi.PostManager.Interfaces;

namespace SimpleUserManagementApi.PostManager.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostController : ControllerBase, IPostController
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
        => _postService = postService;
    
    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAllPostsAsync(CancellationToken ct)
        => Ok(await _postService.GetAllPostsAsync(ct));
    
    [HttpGet("user/{id:guid}")]
    public async Task<ActionResult<List<PostDTO>>> GetAllPostsByUserIdAsync(Guid id, CancellationToken ct)
        => Ok(await _postService.GetAllPostsByUserIdAsync(id, ct));
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostDTO>> GetPostByIdAsync(Guid id, CancellationToken ct)
        => Ok(await _postService.GetPostByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult> AddPostAsync([FromBody] CreatePostDTO post, CancellationToken ct)
    {
        await _postService.AddPostAsync(post, ct);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePostAsync(Guid id, [FromBody] UpdatePostDTO post, CancellationToken ct)
    {
        await _postService.UpdatePostAsync(id, post, ct);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePostAsync(Guid id, CancellationToken ct)
    {
        await _postService.DeletePostAsync(id, ct);
        return Ok();
    }
}