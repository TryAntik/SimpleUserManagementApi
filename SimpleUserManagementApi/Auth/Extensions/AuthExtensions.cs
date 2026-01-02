using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleUserManagementApi.Auth.JWT;
using SimpleUserManagementApi.Exceptions;

namespace SimpleUserManagementApi.Auth.Extensions;


public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

        var key = config["JwtSettings:SecretKey"].Trim();
        var parsedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        
        if(parsedKey == null || parsedKey.Key.Length < 32)
            throw new InvalidOperationException("Invalid secret key");
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = parsedKey
                };
            });
        return services;
    }
}