using Microsoft.EntityFrameworkCore;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    }
}
