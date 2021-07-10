using fitness.Models;
using fitness.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class CustomerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private SecurityManager securityManager = new SecurityManager();
        public CustomerController(DatabaseContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {

            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Accounts.SingleOrDefault(a => a.username.Equals(user.Value));
            return View("index", customer);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var account = new Account();

            return View();
        }

        [HttpPost]
        public IActionResult Register(Account account)
        {
            account.password = BCrypt.Net.BCrypt.HashPassword(account.password);
            account.status = true;
            db.Accounts.Add(account);
            db.SaveChanges();

            var roleAccount = new RoleAccount()
            {
                RoleId = 2,
                AccountId = account.id,
                Status = true
            };
            db.AccountRoles.Add(roleAccount);
            db.SaveChanges();
            return RedirectToAction("login", "customer");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var account = processLogin(email, password);
            if (account != null)
            {
                securityManager.SignIn(this.HttpContext, account, "User_Schema");
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.error = "invalid account";
                return View("login","customer");
            }
        }

        public IActionResult Logout()
        {
            securityManager.SignOut(this.HttpContext, "User_Schema");
            return RedirectToAction("index", "home");
        }

        private Account processLogin(string email, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.email.Equals(email) && a.status == true);
            if (account != null)
            {
                var role = account.RoleAccounts.FirstOrDefault();
                if (role.RoleId == 2 && role.Status == true && BCrypt.Net.BCrypt.Verify(password, account.password))
                {
                    return account;
                }
            }
            return null;
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Accounts.SingleOrDefault(a => a.username.Equals(user.Value));
            return View("profile", customer);
        }

        [HttpPost]
        public IActionResult Profile(Account ac)
        {
            var currentCustomer = db.Accounts.Find(ac.id);
            if (!String.IsNullOrEmpty(ac.password))
            {
                currentCustomer.password = BCrypt.Net.BCrypt.HashPassword(ac.password);
            }
            currentCustomer.fullname = ac.fullname;
            currentCustomer.email = ac.email;
            db.SaveChanges();
            ViewBag.Success = "Bạn đã cập nhật thành công :)";
            return View();
        }
        public IActionResult OrderInfomation()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Accounts.SingleOrDefault(a => a.username.Equals(user.Value));
            ViewBag.invoice = customer.Invoices.OrderByDescending(i => i.Id).ToList();
           
            return View();
        }
        [HttpPost]
        public JsonResult details(int id)
        {
            List<InvoiceDetail> lsInvoiceDetails = db.InvoiceDetails.Where(i => i.InvoiceId == id).ToList();
            List<object> lsobject = new List<object>();
            foreach(var invoiceDetail in lsInvoiceDetails)
            {
                Product product = db.Products.Where(i => i.Id == invoiceDetail.ProductId).SingleOrDefault();
                Photos photo = product.Photoss.SingleOrDefault(p => p.Featured == true);
                lsobject.Add(new
                {
                    ProductId  = invoiceDetail.ProductId,
                    Quantity = invoiceDetail.Quantity,
                    PathPhoto= photo.Name,
                    Price = product.Price,
                    Name = product.Name,
                    Amount = product.Price * invoiceDetail.Quantity
                });
            }
            return Json(new { LsInvoice = lsobject });
        }
    }
}
