using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.DTOs;

public record LoginDTO(
    [MinLength(3), EmailAddress] string Email,
    [MinLength(6), Required] string Password
    );