using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStoreMarket.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Latitude")]
        public double Lat { get; set; }
        [Required]
        [DisplayName("Longitude")]
        public double Long { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 0)]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 0)]
        [DisplayName("City")]
        public string City { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 0)]
        [DisplayName("Address")]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"^\(?(([0-9]{2})|([0-9]{3}))\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The number you entered is not valid")]
        [DisplayName("Phone")]
        public string Telephone { get; set; }
        [DisplayName("Open on Saturday")]
        public Boolean IsSaturday { get; set; }
        // add for relations with Orders
       //public virtual List<Orders> Orders { get; set; }
    }
}
