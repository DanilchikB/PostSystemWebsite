using Microsoft.EntityFrameworkCore;
using MvcUser.Models;
using MvcPost.Models;

namespace MvcUser.Data
{
    public class MvcUserContext : DbContext
    {
        public MvcUserContext (DbContextOptions<MvcUserContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
    }
}