using fitness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
   
    [Route("dashboard")]
    public class DashBoardController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public DashBoardController(DatabaseContext _db)
        {
            this.db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.countInvoices = db.Invoices.Count( i => i.Status == 1);
            ViewBag.countProduct = db.Products.Count();
            ViewBag.countCustomer = db.AccountRoles.Count(ra => ra.RoleId == 2);
            ViewBag.countCategory = db.Categories.Count();

           var listInvoice = from std in db.Invoices join b in db.InvoiceDetails on std.Id equals b.InvoiceId  select new { price = b.Price } ;

            int total = 0;
           
            foreach (var item in listInvoice)
            {
                total += Convert.ToInt32(item.GetType().GetProperty("price").GetValue(item, null));
            }

            ViewBag.Total = total;

            ViewBag.invoiceName = db.Invoices.Where(x => x.Status == 1).ToList().OrderByDescending(i => i.Id).Take(10);

            return View();
        }
    }
}
