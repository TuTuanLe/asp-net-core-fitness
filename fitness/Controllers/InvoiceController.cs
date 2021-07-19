using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Controllers
{
    public class InvoiceController : Controller
    {
        public DatabaseContext db = new DatabaseContext();
        

        public InvoiceController(DatabaseContext _db)
        {
            this.db = _db;
        } 
        public IActionResult Index()
        {
            ViewBag.Invoice = db.Invoices.OrderByDescending(i => i.Id).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Update(string Id_Invoice)
        {

            Invoice invoice = db.Invoices.Find(int.Parse( Id_Invoice));
            invoice.Status = 0;
            invoice.Shipping = 0;

            db.SaveChanges();

            return RedirectToAction("Index", "Invoice");
        }

        [HttpGet]
        public JsonResult LoadData(int page , int pageSize= 3)
        {
            var model = db.Invoices.OrderByDescending(i => i.Id).ToList();
            List<object> ls = new List<object>();
            foreach (var item in model)
            {
                ls.Add(new {
                    Id= item.Id,
                    Name=item.Name,
                    Created=item.Created,
                    Status=item.Status,
                    AccountId=item.AccountId,
                    Shipping= item.Shipping
                });
            }
            int total = model.Count;
            ls = (List<object>)ls.Skip((page - 1) * pageSize).Take(pageSize);

            return Json(new { data = ls, total= total , status = true });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var invoice = db.Invoices.Find(id);

            List<InvoiceDetail> lsDetail = db.InvoiceDetails.Where(d => d.InvoiceId == id).ToList();
            foreach(var x in lsDetail)
            {
                db.Remove(x);
                db.SaveChanges();
            }
            db.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("index", "invoice");
        }
    }
}
