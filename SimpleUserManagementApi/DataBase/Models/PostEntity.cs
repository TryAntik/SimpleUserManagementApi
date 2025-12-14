using System.ComponentModel.DataAnnotations;

namespace SimpleUserManagementApi.Models;

public class PostEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
     
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public UserEntity? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}