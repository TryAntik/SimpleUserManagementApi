using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.DTOs;

public record LoginRequestDTO(
    string Email,
    string Password);