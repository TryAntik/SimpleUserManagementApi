using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.UserManager.DTOs;

public sealed record UserDTO(Guid Id, string Name, string Email, DateTime CreatedAt);

public sealed record CreateUserDTO(
    [MinLength(3), MaxLength(9), Required] string Name, 
    [EmailAddress, MaxLength(30), Required] string Email,
    [Required] string PasswordHash);

public sealed record UpdateUserDTO(
    [MinLength(3), MaxLength(9), Required] string Name, 
    [EmailAddress, MaxLength(30), Required] string Email);