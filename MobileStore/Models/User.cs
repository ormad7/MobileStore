using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStoreMarket.Models
{

    public enum UserType
    {
        Admin,
        Customer
    }
    public class User
    {
        public int Id { get; set; }

        [DisplayName("User Name")]
        [Required, StringLength(maximumLength: 7, MinimumLength = 3, ErrorMessage = "User name must have max lenght 7 and min lenght 3")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password),
         MinLength(6, ErrorMessage = "Password must have atleast 6 characters !")]
        public string Password { get; set; }

        [DisplayName("First Name"), Required]
        [StringLength(20)]
        public string firstName { get; set; }

        [StringLength(20)]
        [DisplayName("Last Name"), Required]
        public string LastName { get; set; }


        [Required, RegularExpression(@"\d+", ErrorMessage = "Phone must contain only digits !")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Phose must have exactly 10 digits !")]
        [Phone]
        public string Phone { get; set; }

        //add validation
        [Required, EmailAddress(ErrorMessage = "Enter the proper Email address !")]
        public string Email { get; set; }

        //[DefaultValue(1)]
        public UserType UserType { get; set; }

    }
}
