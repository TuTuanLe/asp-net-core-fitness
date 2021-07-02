using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class CategoryController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public CategoryController(DatabaseContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            ViewBag.categories = db.Categories.ToList();
            return View();
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("index", "category");
        }

        [HttpGet]
        public IActionResult Add()
        {
            var category = new Category();
            return View("Add", category);
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            category.ParentId = null;
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("index", "category");
        }


        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View("Edit", category);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(Category category)
        {
            var currentCategory = db.Categories.Find(category.id);
            currentCategory.Name = category.Name;
            currentCategory.Status = category.Status;
            db.SaveChanges();
            return RedirectToAction("Index", "category");
        }
    }
}
