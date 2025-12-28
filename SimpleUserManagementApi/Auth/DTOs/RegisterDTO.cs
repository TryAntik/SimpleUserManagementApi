using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.DTOs;

public sealed record RegisterDTO(
    [MinLength(3), MaxLength(9), Required] string Name,
    [MinLength(6), MaxLength(26), Required] string Password,
    [EmailAddress, MaxLength(30), Required] string Email
    );