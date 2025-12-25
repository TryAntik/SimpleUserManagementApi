using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.CRUD.DTOs;

public sealed record PostDTO(Guid Id, string Title, string Content, DateTime CreatedAt, Guid UserId);

public sealed record CreatePostDTO(
    [MinLength(2), Required] string Title,
    [MinLength(2), Required] string Content,
    [Required] Guid UserId);

public sealed record UpdatePostDTO(
    [MinLength(2), Required] string Title,
    [MinLength(2), Required] string Content);