using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.PostManager.DTOs;
using SimpleUserManagementApi.PostManager.Interfaces;

namespace SimpleUserManagementApi.PostManager.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase, IPostController
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
        => _postService = postService;
    
    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAllPostsAsync()
        => Ok(await _postService.GetAllPostsAsync());
    
    [HttpGet("user/{id:guid}")]
    public async Task<ActionResult<List<PostDTO>>> GetAllPostsByUserIdAsync(Guid id)
        => Ok(await _postService.GetAllPostsByUserIdAsync(id));
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostDTO>> GetPostByIdAsync(Guid id)
        => Ok(await _postService.GetPostByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> AddPostAsync([FromBody] CreatePostDTO post)
    {
        await _postService.AddPostAsync(post);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePostAsync(Guid id, [FromBody] UpdatePostDTO post)
    {
        await _postService.UpdatePostAsync(id, post);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePostAsync(Guid id)
    {
        await _postService.DeletePostAsync(id);
        return Ok();
    }
}