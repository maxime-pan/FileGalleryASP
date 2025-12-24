using FileGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace FileGallery.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<FileItem> Files => Set<FileItem>();
    public DbSet<SiteSetting> Settings => Set<SiteSetting>();
    public DbSet<AnonymousUpload> AnonymousUploads => Set<AnonymousUpload>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasMany(c => c.Files).WithOne(f => f.Category!).HasForeignKey(f => f.CategoryId);
        modelBuilder.Entity<Category>().HasIndex(c => c.DisplayOrder);
        modelBuilder.Entity<FileItem>().HasIndex(f => new { f.CategoryId, f.Visible });
        modelBuilder.Entity<SiteSetting>().HasIndex(s => s.Key).IsUnique();
    }
}
