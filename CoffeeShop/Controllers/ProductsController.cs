

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoffeeShop.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace CoffeeShop.Controllers
{
   
    public class ProductsController : Controller
    {
        private ApplicationUserManager _userManager;

        public ProductsController()
        {
            

        }
        public ProductsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
          
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        private ApplicationDbContext db = new ApplicationDbContext();
        [AllowAnonymous]
        // GET: Products
        public ActionResult Index(int group, int? page,string sortoption)
        {
            var pageNumber = page ?? 1;
            var product = db.Product.Include(p => p.Group).Where(x=> x.GroupId == group);

            if (!string.IsNullOrEmpty(sortoption))
            {
                if(sortoption == "1")
                {
                    product = product.OrderBy(x => x.Price);
                }

                if (sortoption == "2")
                {
                    product = product.OrderByDescending(x => x.Price);
                }


            }

            else
            {
                product = product.OrderBy(x => x.ID);
            }


            return View(product.ToPagedList(pageNumber, 3));
        }
        
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [Authorize, Authorize(Roles = "Admin")]
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Group, "ID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize, Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Price,Image,GroupId")] Product product, HttpPostedFileBase productImage)
        {
           
            if (productImage != null)
            {
                var allowedExtensions = new[]
                {
                    ".Jpg",".png", ".jpg", "jpeg", ".JPG",".eps"
                };

                var filename = Path.GetFileName(productImage.FileName);
                var ext = Path.GetExtension(productImage.FileName);
                if (allowedExtensions.Contains(ext))
                {
                     var path = Path.Combine(Server.MapPath("~/Images"), filename);
                     productImage.SaveAs(path);
                     product.Image = "../../Images/" + filename;
                }

            }                
            if (ModelState.IsValid)
            {
               db.Product.Add(product);
               db.SaveChanges();
               return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Group, "ID", "Name", product.GroupId);
            return View(product);
            }
        [Authorize, Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Group, "ID", "Name", product.GroupId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize, Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Price,Image,GroupId")] Product product, HttpPostedFileBase productImage ,string Imagebeforeedit)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null)
                {
                    var allowedExtensions = new[]
                    {
                    ".Jpg",".png", ".jpg", "jpeg", ".JPG"
                };

                    var filename = Path.GetFileName(productImage.FileName);
                    var ext = Path.GetExtension(productImage.FileName);
                    if (allowedExtensions.Contains(ext))
                    {
                        var path = Path.Combine(Server.MapPath("~/Images"), filename);
                        productImage.SaveAs(path);
                        product.Image = "../../Images/" + filename;
                    }

                }
                else
                {
                    product.Image = Imagebeforeedit;
                }



                db.Entry(product).State = EntityState.Modified;
               db.SaveChanges();
                return RedirectToAction("Index");
                }
                ViewBag.GroupId = new SelectList(db.Group, "ID", "Name", product.GroupId);
                return View(product);
            }

        [Authorize, Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize, Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [AllowAnonymous]

        public ActionResult AddToCart(int pid)
        {
            var product = db.Product.Find(pid);
            
            //هیچ محصولی به سبد اضافه نشده
            if (Session["shoppingcart"] == null)
            {
                List<shoppingcartViewModels> product_list = new List<shoppingcartViewModels>();
                product_list.Add(new shoppingcartViewModels {Product = product,quantity=1});
                Session["shoppingcart"] = product_list;
            }

            //قبلا محصولی ثبت شده
            else
            {
                bool exist = false;
                var product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
                foreach (var item in product_list)
                {
                    if(item.Product.ID == product.ID)
                    {
                        //محصول وجود دارد
                        item.quantity = item.quantity + 1;
                        exist = true;
                    }
                }

                if (!exist)
                {
                    product_list.Add(new shoppingcartViewModels { Product = product, quantity = 1 });
                }
               
                Session["shoppingcart"] = product_list;
            }

            return RedirectToAction("shoppingcart");
            
           
        }
         public ActionResult shoppingcart(string status)
        {
            int totalprice = 0;
            List<shoppingcartViewModels> product_list = new List<shoppingcartViewModels>();
            if (Session["shoppingcart"] !=null)
            {
                product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
                foreach (var item in product_list)
                {
                    totalprice = totalprice + (item.Product.Price * item.quantity);
                }
            }
            ViewBag.status = status;
            ViewBag.totalprice = totalprice;
            return View(product_list); 
        }


        public ActionResult DeleteItemFromCart(int pid)
        {
            var product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
            var product = product_list.Where(x => x.Product.ID == pid).FirstOrDefault();
            product_list.Remove(product);
            Session["shoppingcart"] = product_list;
            return RedirectToAction("shoppingcart");
        }

        public ActionResult increasequantity(int pid)
        {
            var product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
            foreach(var item in product_list)
            {
                if (item.Product.ID == pid){

                    item.quantity = item.quantity + 1;
                }
            }
            Session["shoppingcart"] = product_list;
            return RedirectToAction("shoppingcart");
        }


        public ActionResult decreasequantity(int pid)
            {
            var product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
            foreach (var item in product_list)
            {
                if (item.Product.ID == pid)
                {
                    if(item.quantity > 1)
                    {
                        item.quantity = item.quantity - 1;
                    }
                    else
                    {
                        var shoppingcartitem = product_list.Where(x => x.Product.ID == pid).FirstOrDefault();
                        product_list.Remove(shoppingcartitem);
                        break;
                    }
                    
                }
            }
            Session["shoppingcart"] = product_list;
            return RedirectToAction("shoppingcart");
        }

        [ChildActionOnly]
        public ActionResult itemcount()
        {

            int cartitemcount = 0;
            if (Session["shoppingcart"] != null)
            {
                 var product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
                cartitemcount = product_list.Count();
            }
            ViewBag.count = cartitemcount;
            return PartialView();
        }


        public ActionResult submit_order( string payment_method)

        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("shoppingcart", new {status ="شما باید عضو شوید تا بتوانید سفارش خودرا نهایی کنید" });
            }

            ApplicationUser user = UserManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var product_list = Session["shoppingcart"] as List<shoppingcartViewModels>;
            Order order = new Order();
            order.Username = User.Identity.Name;
            order.Phonenumber = user.PhoneNumber;
            order.Address = user.Addrress;
            order.Isdelivered = false;

            int totalprice = 0;
            foreach (var item in product_list)
            {
                totalprice = totalprice + (item.Product.Price * item.quantity);
            }

            order.Totalprice = totalprice;

            if(payment_method == "online")
            {
               //
            }
            if (payment_method == "cash")
            {
                order.Ispayed = false;
            }


            db.Orders.Add(order);
            db.SaveChanges();
            List<OrderDetails> orderDetails_list = new List<OrderDetails>();
            foreach (var item in product_list)
            {
                orderDetails_list.Add(new OrderDetails { OrderId = order.ID, ProductId = item.Product.ID, quantity = item.quantity });
            }
            db.OrderDetails.AddRange(orderDetails_list);
            db.SaveChanges();
            ViewBag.orderid = order.ID;


            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
