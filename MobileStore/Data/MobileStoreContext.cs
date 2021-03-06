using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;

namespace MobileStore.Data
{
    public class MobileStoreContext : DbContext
    {
        public MobileStoreContext (DbContextOptions<MobileStoreContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>()
                 .HasKey(po => new { po.ProductId, po.OrderId });

            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Product)
                .WithMany(o => o.ProductsOrders)
                .HasForeignKey(po => po.ProductId);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Orders)
                .WithMany(o => o.ProductsOrders)
                .HasForeignKey(po => po.OrderId);
        }


        public DbSet<Branch> Branch { get; set; }

        public DbSet<MobileStore.Models.User> User { get; set; }

        public DbSet<MobileStore.Models.Product> Product { get; set; }

        public DbSet<MobileStore.Models.Orders> Orders { get; set; }

        public DbSet<MobileStore.Models.ProductOrder> ProductOrder { get; set; }

        public DbSet<MobileStore.Models.Screen> Screen { get; set; }

        public DbSet<MobileStore.Models.Pc> Pc { get; set; }

        public DbSet<MobileStore.Models.Phone> Phone { get; set; }
    }
}
