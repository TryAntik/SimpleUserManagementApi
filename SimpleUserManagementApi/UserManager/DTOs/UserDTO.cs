using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.UserManager.DTOs;

public sealed record UserDTO(Guid Id, string Name, string Email, DateTime CreatedAt);

public sealed record CreateUserDTO(
    string Name, 
    string Email,
    string Password);

public sealed record UpdateUserDTO(
    string Name, 
    string Email);