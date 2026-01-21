using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.DTOs;

public sealed record RegisterRequestDTO(
    string Name,
    string Password,
    string Email
    );