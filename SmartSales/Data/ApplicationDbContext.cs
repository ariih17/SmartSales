using Microsoft.EntityFrameworkCore;
using SmartSales.Models;

namespace SmartSales.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Order> SO_ORDER { get; set; }
        public DbSet<Customer> COM_CUSTOMER { get; set; }
        public DbSet<Item> SO_ITEM { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relasi Costumer
            modelBuilder.Entity<Order>().HasOne(x => x.Customer)
                .WithMany(x => x.Orders).HasForeignKey(x => x.COM_CUSTOMER_ID);

            //Relasi Item
            modelBuilder.Entity<Order>().HasOne(x => x.Items)
                .WithMany(x => x.Orders).HasForeignKey(x => x.SO_ORDER_ID)
                .HasPrincipalKey(x => x.SO_ORDER_ID);
        }

    }
}
