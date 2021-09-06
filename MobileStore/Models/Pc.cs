
using MobileStore.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Models
{
    public class Pc : Product
    {

        public string Cpu { get; set; }
        public bool Laptop { get; set; }
        public double Ram { get; set; }

        [DisplayName("Graphics Card")]
        public string GraphicsCard { get; set; }
        public double Storage { get; set; }

    }
}
