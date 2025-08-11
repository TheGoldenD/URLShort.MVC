using Microsoft.EntityFrameworkCore;
using URLShort.MVC.Models;

namespace URLShort.MVC.Data;
public class UrlDBContext(DbContextOptions<UrlDBContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrls { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShortUrl>()
            .HasIndex(s => s.ShortCode)
            .IsUnique();

        modelBuilder.Entity<ShortUrl>()
            .Property(s => s.OriginalUrl)
            .IsRequired();

        modelBuilder.Entity<ShortUrl>()
            .Property(s => s.RevokePassword)
            .IsRequired();
    }
}
