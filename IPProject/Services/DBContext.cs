using IPProject.Models;
using System.Data.Entity;

namespace IPProject.Services
{
    public class DBContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<User> Users { get; set; }
    }
}