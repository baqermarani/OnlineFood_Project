using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoffeeShop.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class ProfileViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProfileViewModels
      
        // GET: ProfileViewModels/Details/5
        public ActionResult Details(string id)
        {

            string currentuserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentuserId);
            var viewmodel = new ProfileViewModels();
            viewmodel.ID = currentUser.Id;
            viewmodel.PhoneNumber = currentUser.PhoneNumber;
            viewmodel.Email = currentUser.Email;
            viewmodel.Address = currentUser.Addrress;
            return View(viewmodel);
        }

       
        // GET: ProfileViewModels/Edit/5
        public ActionResult Edit(string id)
        {

            string currentuserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentuserId);
            var viewmodel = new ProfileViewModels();
            viewmodel.ID = currentUser.Id;
            viewmodel.PhoneNumber = currentUser.PhoneNumber;
            viewmodel.Email = currentUser.Email;
            viewmodel.Address = currentUser.Addrress;

            return View(viewmodel);
        }

        // POST: ProfileViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email,PhoneNumber,Address")] ProfileViewModels profileViewModels)
        {

            string currentuserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentuserId);
            currentUser.PhoneNumber = profileViewModels.PhoneNumber;
            currentUser.Addrress = profileViewModels.Address;
            db.SaveChanges();
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
