using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.UserManager.DTOs;

public sealed record UserDTO(Guid Id, string Name, string Email, DateTime CreatedAt);

public sealed record CreateUserDTO(
    [MinLength(3), Required] string Name, 
    [EmailAddress, Required] string Email,
    [Required] string PasswordHash);

public sealed record UpdateUserDTO(
    [MinLength(3), Required] string Name, 
    [EmailAddress, Required] string Email);