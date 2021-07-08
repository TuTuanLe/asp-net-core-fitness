using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db;

        public HomeController(DatabaseContext _db)
        {
            this.db = _db;
        }

    
        public IActionResult Index()
        {
            ViewBag.isHome = true;
            ViewBag.FeaturedProducts = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status).ToList();

            return View(); 
        }

        [HttpGet]
        public IActionResult detailProduct(int id)
        {
            ViewBag.product = db.Products.Find(id);
            return View();
        }


        [HttpPost]
        public JsonResult details(int id)
        {
            var product_db= db.Products.Find(id);
            var product = new {
                Name = product_db.Name,
                Description = product_db.Description,
                Status = product_db.Status,
                CategoryId = product_db.CategoryId,
                Quantity = product_db.Quantity,
                Detail = product_db.Details,
                Price = product_db.Price };

            return Json(new { code = 200, lsProduct = product });

        }
        public IActionResult Index1()
        {
            ViewBag.isHome = true;
            ViewBag.FeaturedProducts = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status).ToList();

            return View();
        }
    }
}
