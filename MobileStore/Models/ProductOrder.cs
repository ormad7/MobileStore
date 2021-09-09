using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Models
{

    public class ProductOrder
    {
        //public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int OrderId { get; set; }

        public Orders Orders { get; set; }

        public int Quantity { get; set; }
    }
}
