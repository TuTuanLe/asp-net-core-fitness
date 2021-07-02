using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.ViewComponents
{
    [ViewComponent(Name ="SlideShow")]
    public class SlideShowViewComponent:ViewComponent
    {
        private DatabaseContext db;
        
        public SlideShowViewComponent(DatabaseContext _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            List<SlideShow> slideShows = db.SlideShows.Where(c => c.Status == true) .ToList();
            return View("Index", slideShows);
        }
    }
}
