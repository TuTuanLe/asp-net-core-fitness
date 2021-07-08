using fitness.Models;
using fitness.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Areas.Admin.Controllers
{

    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private SecurityManager securityManager = new SecurityManager();

        public LoginController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        
        
        [HttpPost]
        [Route("process")]
        public IActionResult Process(string username, string password)
        {
            var a = username;
            return RedirectToAction("index", "DashBoard", new { area = "admin" });
            //var account = processLogin(username, password);
            //if(account != null)
            //{
            //    securityManager.SignIn(this.HttpContext, account);
            //    return RedirectToAction("index","DashBoard", new { area = "admin"});
            //}
            //else
            //{
            //    ViewBag.error = "invalid account";
            //    return View("index");
            //}
           
        }

        private Account processLogin(string username, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.username.Equals(username));
            if (account != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, account.password) )
                {
                    return account;
                }
            }
            return null;
        }

        [Route("signout")]
        public IActionResult signout()
        {
            securityManager.SignOut(this.HttpContext, "Admin_Schema");
            return RedirectToAction("index","login", new { area = "admin" });
        }
        
        
        [Route("accessdenied")]
        public IActionResult accessdenied()
        {
            return View();
        }
    }
}
