using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Auth.RefreshToken;

public record RefreshTokenDTO(
        Guid Id,
        Guid UserId,
        string Token, 
        DateTime Expires);


public record RefreshRequestDTO(
        [Required] string AccessToken,
        [Required] string RefreshToken);

public record RefreshResponseDTO(
        string AccessToken,
        string RefreshToken);        