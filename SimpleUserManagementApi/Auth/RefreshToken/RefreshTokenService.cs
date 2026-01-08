using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;

namespace SimpleUserManagementApi.Auth.RefreshToken;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IOptions<RefreshTokenSettings> _options;

    public RefreshTokenService(ApplicationDbContext dbContext, IOptions<RefreshTokenSettings> options)
    {
        _dbContext = dbContext;
        _options = options;
    }

    public async Task<RefreshTokenDTO> CreateTokenAsync(Guid userId)
    {
        DateTime expires = DateTime.UtcNow.Add(_options.Value.TokenLifeTime);
        
        byte[] tokenBytes = RandomNumberGenerator.GetBytes(64);
        string tokenPlain = Convert.ToBase64String(tokenBytes);
        string tokenHash = BCrypt.Net.BCrypt.HashPassword(tokenPlain);

        Guid tokenId = Guid.NewGuid();
        
        RefreshTokenEntity token = new() // for DBBBBBBBBBBB
        {
            Id = tokenId,
            UserId = userId,
            TokenHash = tokenHash,
            Revoked = default,
            Expires = expires
        };

        RefreshTokenDTO tokenDTO = new(
            tokenId,
            userId,
            tokenPlain,
            expires);
        
        await _dbContext.RefreshTokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
        
        return tokenDTO;
    }

    public async Task<bool> IsValidTokenAsync(string token, Guid userId)
    {
        var activeTokens = await _dbContext.RefreshTokens
            .Where(t =>
                t.UserId == userId
                && !t.Revoked
                && t.Expires > DateTime.UtcNow)
            .ToListAsync();

        return activeTokens.Any(t 
            => BCrypt.Net.BCrypt.Verify(token, t.TokenHash));
    }

    public async Task<RefreshTokenEntity> RevokeTokenAsync(Guid tokenId)
    {
        var tokenDb = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Id == tokenId);
        if (tokenDb is null) throw new NotFoundException("Token was not found");
        
        tokenDb.Revoked = true;

        await _dbContext.SaveChangesAsync();
        return tokenDb;
    }

    public async Task<int> RevokeAllUserTokensAsync(Guid userId)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(t => t.UserId == userId && t.Revoked == false)
            .ToListAsync();
        
        foreach (var token in tokens)
            token.Revoked = true;

        await _dbContext.SaveChangesAsync();
        return tokens.Count;
    }
}


















