using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;

namespace BlogAPI.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); ;
        }
    }
}
