using fitness.Helpers;
using fitness.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MoMo;
using Newtonsoft.Json.Linq;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (cart == null)
                ViewBag.countItems = 0;
            else
                ViewBag.countItems = cart.Count;

            return View();
        }


        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var product = db.Products.Find(id);
            var photo = ((Models.Product)product).Photoss.SingleOrDefault(p => p.Featured == true);
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
            }
            return RedirectToAction("index", "Cart");
        }

        [HttpPost]
        public JsonResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            var photo = ((Models.Product)product).Photoss.SingleOrDefault(p => p.Featured == true);
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
            for (var i = 0; i < cart.Count; i++)
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

        [HttpPost]
        public JsonResult Update(int id, int quantity)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = exists(id, cart);
            cart[index].Quantity = quantity;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Json(new { carts = cart });
        }

        public IActionResult CheckOut()
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart == null)
                ViewBag.countItems = 0;
            else
                ViewBag.countItems = cart.Count;
            var user = User.FindFirst(ClaimTypes.Name);
            var account = new Models.Account();
            if (user != null)
            {
                account = db.Accounts.SingleOrDefault(a => a.username.Equals(user.Value));
            }
            ViewBag.User = account;
            return View(account);
        }
        [HttpPost]
        public IActionResult ProcessCheckOut(Models.Account ac, bool customer_pick_at_location, string payment_method_id)
        {
            Random r = new Random();
            if (payment_method_id == "COD")
            {
                var invoice = new Models.Invoice()
                {
                    Name = "#IF_" + r.Next(1000, 9999) + "_" + Convert.ToChar(Convert.ToInt32(r.Next(65, 87))) + "INVOICE DELIVERY",
                    Created = DateTime.Now,
                    Status = 1,
                    AccountId = ac.id,
                    Shipping = 1
                };
                db.Invoices.Add(invoice);
                db.SaveChanges();
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                foreach (var item in cart)
                {
                    var invoiceDetails = new InvoiceDetail()
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    db.InvoiceDetails.Add(invoiceDetails);
                    db.SaveChanges();
                }
                HttpContext.Session.Remove("cart");

                ViewBag.ac = ac;
                ViewBag.location = customer_pick_at_location;
                ViewBag.Payment_COD_or_momo = payment_method_id;
                return RedirectToAction("OrderInfomation", "customer");
            }
            else if (payment_method_id == "momo")
            {
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMOW6RA20201226";
                string accessKey = "omXMiAO31qfCgaD5";
                string serectkey = "bhtUeiWK8aRdk2sAWkqLx5eQfm9oYsuD";
                string orderInfo = "Thanh toan don hang";
                string returnUrl = "https://localhost:44395/cart/notify";
                string notifyurl = "https://localhost:44395/cart/return";
                string amount = "500000";
                string orderid = Guid.NewGuid().ToString(); ;
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                string rawHash = "partnerCode=" +
                   partnerCode + "&accessKey=" +
                   accessKey + "&requestId=" +
                   requestId + "&amount=" +
                   amount + "&orderId=" +
                   orderid + "&orderInfo=" +
                   orderInfo + "&returnUrl=" +
                   returnUrl + "&notifyUrl=" +
                   notifyurl + "&extraData=" +
                   extraData;
                MoMoSecurity crypto = new MoMoSecurity();
                string signature = crypto.signSHA256(rawHash, serectkey);

                JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId", orderid },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyurl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }

                };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);
                return Redirect(jmessage.GetValue("payUrl").ToString());
            }else if(payment_method_id == "stripe")
            {
                return RedirectToAction("paymentWithStripe","cart");
            }
            return View();
        }

        public IActionResult ReturnUrl()
        {
            ViewBag.message = "Thanh toan that bai";
            return View();
        }

        [HttpGet]
        public IActionResult notify(string partnerCode, string accessKey, string amount, string orderId, string orderInfo, string orderType, string transId, string message, string responseTime, string status_code)
        {
            string param = "";
            param = "partner_code=" + partnerCode +
                "&access_key=" + accessKey +
                "&amount=" + amount +
                "&order_id=" + orderId +
                "&order_info=" + orderInfo +
                "&order_type=" + orderType +
                "&transaction_id=" + transId +
                "&message=" + message +
                "&response_time=" + responseTime +
                "&status_code=" + status_code;
            MoMoSecurity crypto = new MoMoSecurity();
            //string serectkey = "bhtUeiWK8aRdk2sAWkqLx5eQfm9oYsuD";
            string _status_code = status_code;
            if (_status_code != "0")
            {
                //thất bại cập nhật lại đơn hàng
            }
            else
            {
                //thành công cập nhật lại đơn hàng
            }
            return RedirectToAction("ReturnUrl", "cart");
        }


        public IActionResult paymentWithStripe()
        {
            return View();
        }
        public IActionResult Process_Stripe(string stripeEmail, string stripeToken)
        {
            Random r = new Random();
            var user = User.FindFirst(ClaimTypes.Name);
            var cus = db.Accounts.SingleOrDefault(a => a.username.Equals(user.Value));

            var invoice = new Models.Invoice()
            {
                Name = "#IO_"+r.Next(1000,9999) +"_"+ Convert.ToChar(Convert.ToInt32(r.Next(65, 87))) +"INVOICE ONLINE",
                Created = DateTime.Now,
                Status = 1,
                AccountId = cus.id,
                Shipping=0
            };
            db.Invoices.Add(invoice);
            db.SaveChanges();
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

            decimal total = 0;
            string Description = "#" + r.Next(10000, 20000)+ "---------";
            string order = "";

            foreach (var item in cart)
            {
                var invoiceDetails = new InvoiceDetail()
                {
                    InvoiceId = invoice.Id,
                    ProductId = item.Id,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                Description += item.Name;
                total += item.Price;
                order += item.Name + "  " + item.Price + "   " + item.Quantity + "   \n"; 

                db.InvoiceDetails.Add(invoiceDetails);
                db.SaveChanges();
            }
            HttpContext.Session.Remove("cart");


            var customers = new CustomerService();
            var charges = new ChargeService();
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount= (int)total,
                Description= Description,
                Currency="usd",
                Customer = customer.Id,
                ReceiptEmail= stripeEmail,
                Metadata = new Dictionary<string, string>()
                {
                    {"order",order },
                    {"postcode","#"+r.Next(10000,20000) }
                }
            });
            if(charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                return RedirectToAction("OrderInfomation", "customer");
            }
            else
            {
                return RedirectToAction("ReturnUrl", "cart");
            }


        }
        [HttpGet]
        public IActionResult test3(string hoten, string ten, int sdt)
        {
            return Content("HO TEN: "+hoten+"TEN: "+ten);
        }
    }


    
}
