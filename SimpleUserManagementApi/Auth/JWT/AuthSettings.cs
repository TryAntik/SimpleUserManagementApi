namespace SimpleUserManagementApi.Auth.JWT;

public class AuthSettings
{
    public TimeSpan Expires { get; set; }
    public string SecretKey { get; set; } 
}