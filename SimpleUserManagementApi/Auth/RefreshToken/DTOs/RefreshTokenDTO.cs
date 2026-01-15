using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.RefreshToken;

public record RefreshTokenDTO(
    Guid Id,
    Guid UserId,
    string Token, 
    DateTime Expires);


public record RefreshRequestDTO(
    [Required] string AccessToken,
    [Required] RefreshTokenDTO RefreshTokenDto);

public record RefreshResponseDTO(
    string AccessToken,
    string RefreshToken);