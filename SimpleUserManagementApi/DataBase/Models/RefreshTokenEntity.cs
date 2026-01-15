namespace SimpleUserManagementApi.DataBase.Models;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = String.Empty;
    public bool Revoked { get; set; }
    public DateTime Expires { get; set; }
}