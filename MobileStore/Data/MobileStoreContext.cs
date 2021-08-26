using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileStoreMarket.Models;

namespace MobileStore.Data
{
    public class MobileStoreContext : DbContext
    {
        public MobileStoreContext (DbContextOptions<MobileStoreContext> options)
            : base(options)
        {
        }

        public DbSet<MobileStoreMarket.Models.Branch> Branch { get; set; }

        public DbSet<MobileStoreMarket.Models.User> User { get; set; }
    }
}
