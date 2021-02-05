using IdentitySample.Models;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.newstproducts = db.Product.OrderByDescending(x => x.ID).Take(4).ToList();
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult _nav_partial()
        {
            ViewBag.groups = db.Group.ToList();
            var model = db.Group.ToList();



            return PartialView(model);
        }





        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
