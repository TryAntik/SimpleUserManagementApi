using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.Auth.RefreshToken;

public interface IRefreshTokenService
{
    Task<RefreshTokenDTO> CreateTokenAsync(Guid userId);
    Task<bool> IsValidTokenAsync(string token, Guid userId);
    Task<RefreshTokenEntity> RevokeTokenAsync(Guid tokenId);
    Task<int> RevokeAllUserTokensAsync(Guid userId);
}