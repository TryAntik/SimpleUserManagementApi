using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth;

public sealed record RegisterDTO(
    [MinLength(3), Required] string Name,
    [MinLength(6), Required] string Password,
    [EmailAddress, Required] string Email
    );