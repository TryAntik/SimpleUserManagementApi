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
        services.AddScoped<IJwtService, JwtService>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var key = configuration["JwtSettings:SecretKey"];
        
        if(string.IsNullOrWhiteSpace(key) || key.Length < 32 || key.Contains(' '))
            throw new InvalidOperationException("Invalid key format");
        
        var keyParsed = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = keyParsed
                };
            });

        services.AddAuthorization(options =>
            options.AddPolicy("AdminAccess", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin")));
        
        return services;
    }
} 