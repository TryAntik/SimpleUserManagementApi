using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.Auth.RefreshToken;

public interface IRefreshTokenService
{
    Task<RefreshTokenDTO> CreateTokenAsync(Guid userId, CancellationToken ct);
    Task<bool> IsValidTokenAsync(RefreshTokenDTO dto, CancellationToken ct);
    Task<RefreshTokenEntity> RevokeTokenAsync(Guid tokenId, CancellationToken ct);
    Task<int> RevokeAllUserTokensAsync(Guid userId, CancellationToken ct);
    Task<RefreshTokenEntity?> GetTokenAsync(RefreshTokenDTO dto, CancellationToken ct);
}