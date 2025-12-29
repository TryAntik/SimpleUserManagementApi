using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.Auth.JWT;

public class JwtService : IJwtService
{
    private readonly IOptions<AuthSettings> _options;

    public JwtService(IOptions<AuthSettings> options)
        => _options = options;

    public string GenerateToken(UserEntity user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(
            new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_options.Value.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_options.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            });
            
        return handler.WriteToken(token);
    }
}

public interface IJwtService
{
    string GenerateToken(UserEntity user);
}