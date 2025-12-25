using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Models;

public class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public List<PostEntity> Posts { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}