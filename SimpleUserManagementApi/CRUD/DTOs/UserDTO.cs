using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.CRUD.DTOs;

public sealed record UserDTO(int Id, string Name, string Email, DateTime CreatedAt);

public sealed record CreateUserDTO(
    [MinLength(2), Required] string Name, 
    [EmailAddress, Required] string Email);

public sealed record UpdateUserDTO(
    [MinLength(2), Required] string Name, 
    [EmailAddress, Required] string Email);