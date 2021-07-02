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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("index1")]
        public IActionResult Index1(string username, string password)
        {
            ViewBag.username = username;
            ViewBag.password = password;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    
    }
}
