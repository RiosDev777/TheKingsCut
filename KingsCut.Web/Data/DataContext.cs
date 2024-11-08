using KingsCut.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingsCut.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Service>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
