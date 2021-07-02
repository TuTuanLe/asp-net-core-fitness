using fitness.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class SlideShowController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
     
        private IHostingEnvironment ihostingEnviroment;

        public SlideShowController(DatabaseContext _db, IHostingEnvironment hostingEnvironment)
        {
            this.db = _db;
            this.ihostingEnviroment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.SlideShows = db.SlideShows.ToList();

            return View();
        }

        public IActionResult Add()
        {
            SlideShow slide = new SlideShow();
            return View("Add",slide);
        }

        [HttpPost]
        public IActionResult Add(SlideShow slideshow, IFormFile photo)
        {
            var path = Path.Combine(this.ihostingEnviroment.WebRootPath, "SlideShows", photo.FileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            slideshow.PathImage = photo.FileName;
            db.SlideShows.Add(slideshow);
            db.SaveChanges();

            return RedirectToAction("Index", "SlideShow");
        }
    }
}
