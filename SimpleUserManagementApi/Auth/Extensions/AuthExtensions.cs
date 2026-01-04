using System.ComponentModel.Design;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleUserManagementApi.Auth.JWT;
using SimpleUserManagementApi.Exceptions;

namespace SimpleUserManagementApi.Auth.Extensions;


public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJwtService, JwtService> ();
        services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

        var key = config["JwtSettings:SecretKey"];
        var parsedKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"].Trim()));

        if (key.Contains(' ') || key.Length < 32)
            throw new InvalidOperationException("Invalid format for security key");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = parsedKey
                };
            });
        return services;
    }
}