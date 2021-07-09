using fitness.Helpers;
using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.ViewComponents
{

    [ViewComponent(Name ="CartLeft")]
    public class CartLeftViewComponent: ViewComponent
    {
       
        public IViewComponentResult Invoke()
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if(cart == null)
            {
                ViewBag.countItems = 0;
            }
            else
                ViewBag.countItems = cart.Count;

            return View("Index");
        }
    }
}
