using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Data
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        { }

        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("photos");
                
                // Configure column names to match PostgreSQL convention (lowercase)
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FileName).HasColumnName("filename");
                entity.Property(e => e.PrevUrl).HasColumnName("prevurl");
                entity.Property(e => e.BlobUrl).HasColumnName("bloburl");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.LikeCount).HasColumnName("likecount");
            });
        }
    }
}
