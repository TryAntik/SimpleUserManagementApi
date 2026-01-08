using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.DTOs;

public record LoginRequestDTO(
    [Required, MaxLength(30)] string Email,
    [Required, MinLength(6), MaxLength(26)] string Password);