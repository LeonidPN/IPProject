using IPProject.Models;
using System.Data.Entity;

namespace IPProject.Services
{
    public class DBContext : DbContext
    {
        public DbSet<Category> Category { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<User> User { get; set; }
    }
}