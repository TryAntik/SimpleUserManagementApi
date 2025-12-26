using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.Auth.JWT;

public class JwtService(IOptions<AuthSettings> authSettings)
{
    public string GenerateToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Name", user.Name),
            new Claim("Email", user.Email),
        };
        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(authSettings.Value.Expires),
            claims: claims,
            signingCredentials:
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(authSettings.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}