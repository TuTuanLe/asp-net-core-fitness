using fitness.Models;
using fitness.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class ProductController : Controller
    {
        [Obsolete]
        private IHostingEnvironment ihostingEnviroment;
        private DatabaseContext db = new DatabaseContext();

        [Obsolete]
        public ProductController(DatabaseContext _db, IHostingEnvironment hostingEnvironment)
        {
            this.db = _db;
            this.ihostingEnviroment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.Products = db.Products.ToList();
            return View();
        }
        [HttpGet]
 
        public IActionResult Add()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.Product = new Product();
            productViewModel.Categories = new SelectList(db.Categories.ToList(), "id", "Name");
            
            return View("Add", productViewModel);
        }

        [HttpPost]
        [Obsolete]
        public IActionResult Add(ProductViewModel productViewModel, IFormFile photo)
        {
            string PathImgPlace = DateTime.Now.ToString("MMddyyyyhhmmss");
            var path = Path.Combine(this.ihostingEnviroment.WebRootPath, "product", PathImgPlace + photo.FileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            

            db.Products.Add(productViewModel.Product);
            db.SaveChanges();
            var product_test = db.Products.SingleOrDefault(p => p.Price == productViewModel.Product.Price && p.Name.Equals(productViewModel.Product.Name) &&  p.CategoryId == productViewModel.Product.CategoryId );

            var defaultPhoto = new Photos
            {
                Name = PathImgPlace + photo.FileName,
                Status = true,
                ProductId = product_test.Id,
                Featured = true
            };
            db.Photoss.Add(defaultPhoto);
            db.SaveChanges();
            return RedirectToAction("index", "Product");
        }
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product= db.Products.Find(id);
            var photo = db.Photoss.SingleOrDefault(productid => productid.ProductId == id);
            db.Remove(photo);
            db.SaveChanges();
            db.Remove(product);
            db.SaveChanges();

            return RedirectToAction("index", "product");
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            ViewBag.Categories = db.Categories.ToList();
            Product product = db.Products.Find(id);
            return View("update", product);
        }
        [HttpPost]
        [Route("update/{id}")]
        public IActionResult Update(Product product)
        {

            Product temp = db.Products.Find(product.Id);
            temp.Description = product.Description;
            temp.Name = product.Name;
            temp.Status = product.Status;
            temp.Price = product.Price;
            temp.Quantity = product.Quantity;
            temp.Details = product.Details;
            temp.CategoryId = product.CategoryId;
            db.SaveChanges();

            return RedirectToAction("index", "Product");
        }

    }
}
