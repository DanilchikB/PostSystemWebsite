using Microsoft.EntityFrameworkCore;
using MvcUser.Models;
using MvcPost.Models;
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
    }
}