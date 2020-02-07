using Microsoft.EntityFrameworkCore;
using MvcUser.Models;
using MvcPost.Models;
using MvcLike.Models;
using MvcFeedback.Models;

namespace MvcDataContext.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Feedback> Feedback { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>()
            .HasKey(t => new { t.UserId, t.PostId });
 
        modelBuilder.Entity<Like>()
            .HasOne(sc => sc.User)
            .WithMany(s => s.Likes)
            .HasForeignKey(sc => sc.UserId);
 
        modelBuilder.Entity<Like>()
            .HasOne(sc => sc.Post)
            .WithMany(c => c.Likes)
            .HasForeignKey(sc => sc.PostId);
    }
    }
}