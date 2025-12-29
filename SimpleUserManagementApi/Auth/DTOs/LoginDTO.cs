using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.DTOs;

public record LoginDTO(
    [MinLength(3), MaxLength(30), EmailAddress] string Email,
    [MinLength(6), MaxLength(26), Required] string Password
    );