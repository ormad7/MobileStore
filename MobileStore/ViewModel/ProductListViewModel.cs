using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MobileStore.Models;

namespace MobileStore.ViewModel
{
    public class ProductListViewModel
    {

        public IEnumerable<Product> Products { get; set; }
        public string CurrentProductType { get; set; }
        public string CurrentProductName { get; set; }

    }
}
