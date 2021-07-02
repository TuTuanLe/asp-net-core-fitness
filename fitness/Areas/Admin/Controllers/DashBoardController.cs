using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/dashboard")]
    public class DashBoardController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
