using Microsoft.EntityFrameworkCore;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.DataBase;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }  
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
}