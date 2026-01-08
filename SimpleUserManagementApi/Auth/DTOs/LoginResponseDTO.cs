namespace SimpleUserManagementApi.Auth.DTOs;

public record LoginResponseDTO(
    string AccessToken,
    string RefreshToken
    );