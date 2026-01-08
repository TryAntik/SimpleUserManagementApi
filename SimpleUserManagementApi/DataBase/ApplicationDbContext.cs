using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.DataBase;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }  
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Role)
            .HasConversion(new EnumToStringConverter<UserRole>());
        base.OnModelCreating(modelBuilder);
    }
}