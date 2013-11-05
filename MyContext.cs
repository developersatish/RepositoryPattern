using System.Data.Entity;
using Portal.Domain;

namespace Portal.Infrastructure
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("MyCon")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}