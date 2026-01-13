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
    
    public Guid GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);

        var userId = jsonToken.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier);

        if (userId is null) throw new NotFoundException("ID Not found");

        return Guid.Parse(userId.Value);
    }
}

public interface IJwtService
{
    string GenerateToken(UserEntity user);
    Guid GetUserIdFromToken(string token);
}