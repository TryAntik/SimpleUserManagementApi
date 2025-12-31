namespace SimpleUserManagementApi.Auth.JWT;

public class JwtSettings
{
    public TimeSpan TokenLifeTime { get; set; }
    public string SecretKey { get; set; } 
}