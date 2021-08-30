using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public User User { get; set; }

        public double TotalPrice { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Order Date")]
        public DateTime DateOfOrder { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [StringLength(maximumLength: 16, MinimumLength = 3, ErrorMessage = "Address must have max lenght 16 and min lenght 3")]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [StringLength(maximumLength: 16, MinimumLength = 3, ErrorMessage = "City must have max lenght 16 and min lenght 3")]
        public string City { get; set; }

        [DisplayName("Zip Code")]
        [Required, RegularExpression(@"\d+", ErrorMessage = "Phone must contain only digits !")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Phose must have exactly 10 digits !")]
        public string Zip { get; set; }

        public int CreditCard { get; set; }

        //public ICollection<ProductOrder> ProductsOrders { get; set; }


    }
}