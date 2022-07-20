using Grocery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grocery.Controllers
{
    public class LoginVendorController : Controller
    {
        db_vendorEntities db = new db_vendorEntities();
        // GET: LoginVendor
        [HttpGet]
        public ActionResult LoginVendor()
        {
            return View();
        }

        public ActionResult LoginVendor(LoginViewModel model)
        {
            var user = (from v in db.tbl_vendor
                        where v.Email.Equals(model.Email) &&
                        v.Password.Equals(model.Password)
                        select v).SingleOrDefault();
            if (user != null)
            {
                ViewBag.SuccessMessage = "Rgistration Successful!";
                Session["IdSS"] = user.BIN;
                Session["NameSS"] = user.Name;
                return RedirectToAction("VendorProfile","Vendor",user);
                //return View();
                //ViewBag.DuplicateMessage = "User with this email already exist";
                //return View("Vendor", model);
            }
            ViewBag.NotFoundMessage = "User Not Found.";
            return View("LoginVendor", new LoginViewModel());
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("LoginVendor","LoginVendor", new tbl_vendor());
        }
    }
}