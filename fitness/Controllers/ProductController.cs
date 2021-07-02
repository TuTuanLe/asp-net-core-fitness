using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class ProductController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public ProductController(DatabaseContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {
            ViewBag.Products = db.Products.ToList();
            return View();
        }
    }
}
