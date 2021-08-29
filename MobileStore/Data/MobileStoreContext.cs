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

        public DbSet<MobileStore.Models.Branch> Branch { get; set; }

        public DbSet<MobileStore.Models.User> User { get; set; }

        public DbSet<MobileStore.Models.Product> Product { get; set; }
    }
}
