using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileStore.Data;

namespace MobileStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly MobileStoreContext _context;

        public StatisticsController(MobileStoreContext context)
        {
            _context = context;
        }

        // GET: Statistics
        public IActionResult Index()
        {
            return View();
        }

        // Get gross income generated by product types
        [HttpGet]
        public JsonResult TotalIncomeByProductType()
        {


            var statisResult = _context.ProductOrder.Join(_context.Product,
                (order => order.Product.ProductType),
                (productType => productType.ProductType),
                (o, t) => new
                {
                    productTypeId = t.Id,
                    productTypeName = t.ProductType,
                    productPrice = o.Product.Price,
                    orderQuantity = o.Quantity
                })

            .GroupBy(t => t.productTypeName)
            .Select(o => new
            {
                Income = o.Sum(order => order.orderQuantity * order.productPrice),
                Name = o.Key
            });

            return Json(statisResult);
        }
        [HttpGet]
        public ActionResult TotalOrdersBySupplier()
        {

            var statisResult = _context.ProductOrder.Join(_context.Product,
                (order => order.Product.Company),
                (productCompany => productCompany.Company),
                (o, s) => new
                {
                    productTypeId = s.Id,
                    productCompany = s.Company,
                    productPrice = o.Product.Price,
                    orderQuantity = o.Quantity
                })

            .GroupBy(t => t.productCompany)
            .Select(o => new
            {
                Amount = o.Sum(order => order.orderQuantity),
                Name = o.Key
            });

            return Json(statisResult);
        }
    }

}

