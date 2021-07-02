using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.ViewComponents
{

    [ViewComponent(Name ="Category")]
    public class CategoryViewComponent: ViewComponent
    {
        private DatabaseContext db;

        public CategoryViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        //async Task<IViewComponentResult>
        public IViewComponentResult Invoke()
        {
            List<Category> categories = db.Categories.Where(c => c.Status).ToList();
            return View("Index", categories);
        }
    }
}
