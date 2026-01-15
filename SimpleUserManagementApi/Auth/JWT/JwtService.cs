using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;

namespace SimpleUserManagementApi.Auth.JWT;

public class JwtService : IJwtService
{
    private readonly IOptions<JwtSettings> _options;

    public JwtService(IOptions<JwtSettings> options)
        => _options = options;

    public string GenerateToken(UserEntity user)
    {
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = creds,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.Value.TokenLifeTime),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool TryGetUserIdFromToken(string token, out Guid userId)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey));

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = secretKey
            }, out _);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out userId)) return true;

            userId = default;
            return false;
        }
        catch
        {
            userId = default;
            return false;
        }
    }
}

public interface IJwtService
{
    string GenerateToken(UserEntity user);
    bool TryGetUserIdFromToken(string token, out Guid userId);
}