using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineStore.Entities;
using BCryptNet = BCrypt.Net.BCrypt;

namespace OnlineStore.Helpers
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = -1,
                    Username = "Mangager",
                    Email = "manager@gmail.com",
                    PasswordHash = BCryptNet.HashPassword("Manager@111"),
                    Role = Role.Manager
                }
            );
        }
    }
}