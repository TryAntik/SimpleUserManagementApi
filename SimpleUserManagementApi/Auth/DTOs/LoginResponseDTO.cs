using SimpleUserManagementApi.Auth.RefreshToken;

namespace SimpleUserManagementApi.Auth.DTOs;

public record LoginResponseDTO(
    string AccessToken,
    RefreshTokenDTO RefreshToken
    );