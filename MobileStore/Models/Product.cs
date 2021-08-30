using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("Product Type")]
        public string ProductType { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Released")]
        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        [DisplayName("Size Of Screen")]
        public double Size { get; set; }


        [Required]
        public string img { get; set; }

        public ICollection<ProductOrder> ProductsOrders { get; set; }

    }
}
