using System.ComponentModel.Design;
using System.Security.Claims;
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
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["JwtSettings:SecretKey"]?.Trim();
        var parsedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        if (key.Contains(' ') || key.Length < 32)
            throw new InvalidOperationException("invalid key");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = parsedKey
                };
            });
        return services;

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminAccess", policy =>
                policy.RequireRole("Admin"));
        });
    }
}