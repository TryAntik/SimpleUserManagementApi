using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.Models;
using SimpleUserManagementApi.PostManager.DTOs;
using SimpleUserManagementApi.PostManager.Interfaces;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.PostManager.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostService(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<List<PostDTO>> GetAllPostsAsync()
    {
        var entityPosts = await _postRepository.GetAllPostsAsync();

        return entityPosts.Select(a => new PostDTO(
            a.Id,
            a.Title,
            a.Content,
            a.CreatedAt,
            a.UserId
        )).ToList();
    }

    public async Task<PostDTO?> GetPostByIdAsync(Guid id)
    {
        var entity = await _postRepository.GetPostByIdAsync(id);

        if (entity is null) throw new NotFoundException(
            $"post with id {id} not found");
        
        return new PostDTO(
            entity.Id,
            entity.Title,
            entity.Content,
            entity.CreatedAt,
            entity.UserId);
    }
 
    public async Task<List<PostDTO>> GetAllPostsByUserIdAsync(Guid userId)
    {
        var entityPosts = await _postRepository.GetAllPostsByUserIdAsync(userId);
    
        if (entityPosts is null || !entityPosts.Any())
            throw new NotFoundException($"Posts for user {userId} not found");

        return entityPosts.Select(a => new PostDTO(
            a.Id,
            a.Title,
            a.Content,
            a.CreatedAt,
            a.UserId)).ToList();
    }

    public async Task AddPostAsync(CreatePostDTO post)
    {
        var userExist = await _userRepository.GetUserByIdAsync(post.UserId);

        if (userExist is null) throw new NotFoundException(
            $"user with id {post.UserId} not found");
        
        var postEntity = new PostEntity
        {
            Title = post.Title,
            Content = post.Content,
            UserId = post.UserId 
        };

        await _postRepository.AddPostAsync(postEntity);
    }

    public async Task UpdatePostAsync(Guid id, UpdatePostDTO post)
    {
        var postEntity = await _postRepository.GetPostByIdAsync(id);
        
        if(postEntity is null) throw new NotFoundException(
            $"post with id {id} not found");

        postEntity.Title = post.Title;
        postEntity.Content = post.Content;
        
        await _postRepository.UpdatePostAsync(postEntity);
    }

    public async Task DeletePostAsync(Guid id)
        => await _postRepository.DeletePostAsync(id);
}