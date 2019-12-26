using Microsoft.EntityFrameworkCore;

namespace MvcUser.Models
{
    public class MvcUserContext : DbContext
    {
        public MvcUserContext (DbContextOptions<MvcUserContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}