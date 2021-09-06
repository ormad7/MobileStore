using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;
using MobileStoreMarket.Models;

namespace MobileStore.Data
{
    public class MobileStoreContext : DbContext
    {
        public MobileStoreContext (DbContextOptions<MobileStoreContext> options)
            : base(options)
        {
        }

        public DbSet<MobileStore.Models.Branch> Branch { get; set; }

        public DbSet<MobileStore.Models.User> User { get; set; }

        public DbSet<MobileStore.Models.Product> Product { get; set; }
        //public IQueryable<Orders> Orders { get; internal set; }
        public DbSet<MobileStore.Models.Orders> Orders { get; set; }
        //public IQueryable<Orders> Orders { get; internal set; }
        public DbSet<MobileStore.Models.ProductOrder> ProductOrder { get; set; }
        //public IQueryable<Orders> Orders { get; internal set; }
        public DbSet<MobileStore.Models.Screen> Screen { get; set; }
        //public IQueryable<Orders> Orders { get; internal set; }
        public DbSet<MobileStoreMarket.Models.Pc> Pc { get; set; }
        //public IQueryable<Orders> Orders { get; internal set; }
        public DbSet<MobileStore.Models.Phone> Phone { get; set; }
    }
}
