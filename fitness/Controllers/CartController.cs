using fitness.Helpers;
using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace fitness.Controllers
{
    public class CartController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public CartController(DatabaseContext _db)
        {
            this.db = _db;
        }
        public IActionResult Index()
        {
             List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.countItems = cart.Count;

            return View();
        }


        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var product = db.Products.Find(id);  
            var photo = ((Product)product).Photoss.SingleOrDefault(p => p.Featured == true);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Details = product.Details,
                    Price = product.Price,
                    Quantity = 1,
                    Status = product.Status,
                    PathPhoto = photo.Name
                }) ;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = exists(id, cart);
                if(index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Details = product.Details,
                        Price = product.Price,
                        Quantity = 1,
                        Status = product.Status,
                        PathPhoto = photo.Name
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("index", "Cart");
        }

        [HttpPost]
        public JsonResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            var photo = ((Product)product).Photoss.SingleOrDefault(p => p.Featured == true);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Details = product.Details,
                    Price = product.Price,
                    Quantity = 1,
                    Status = product.Status,
                    PathPhoto = photo.Name
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Json(new { code = 200, carts = cart });
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = exists(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Details = product.Details,
                        Price = product.Price,
                        Quantity = 1,
                        Status = product.Status,
                        PathPhoto = photo.Name
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Json(new { code = 200, carts = cart });
            }

         }

        private int exists(int id, List<Item> cart)
        {
            for (var i = 0; i< cart.Count; i++)
            {
                if (cart[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = exists(id, cart);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("index", "Cart");
        } 
        [HttpPost]
        public JsonResult RemoveItem(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = exists(id, cart);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Json(new { carts = cart });

        }
    }
}
