namespace SimpleUserManagementApi.Auth.JWT;

public class AuthSettings
{
    public TimeSpan TokenLifeTime { get; set; }
    public string SecretKey { get; set; } 
}