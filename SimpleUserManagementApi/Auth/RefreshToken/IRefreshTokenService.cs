using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.Auth.RefreshToken;

public interface IRefreshTokenService
{
    Task<RefreshTokenDTO> CreateTokenAsync(Guid userId, CancellationToken ct);
    Task<bool> IsValidTokenAsync(string token, Guid userId, CancellationToken ct);
    Task<RefreshTokenEntity> RevokeTokenAsync(Guid tokenId, CancellationToken ct);
    Task<int> RevokeAllUserTokensAsync(Guid userId, CancellationToken ct);
    Task<RefreshTokenEntity?> GetTokenAsync(string token, Guid userId, CancellationToken ct);
}