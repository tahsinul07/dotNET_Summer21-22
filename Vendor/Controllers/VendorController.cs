using Grocery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grocery.Controllers
{
    public class VendorController : Controller
    {
        db_vendorEntities db = new db_vendorEntities();
        // GET: Vendor
        [HttpGet]
        public ActionResult Vendor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Vendor(tbl_vendor model)
        {

            if (db.tbl_vendor.Any(x=>x.Email == model.Email))
            {
                ViewBag.DuplicateMessage = "User with this email already exist";
                return View("Vendor",model);
            }
            if(ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                fileName = fileName+ DateTime.Now.ToString("yymmssffff") + extension;
                model.Photo = fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                model.ImageFile.SaveAs(fileName);
                tbl_vendor entity = new tbl_vendor();
                entity.Name = model.Name;
                entity.Password = model.Password;
                entity.Address = model.Address;
                entity.Email = model.Email;
                entity.Mobile = model.Mobile;
                entity.Photo = model.Photo;
                entity.Password = model.Password;

                db.tbl_vendor.Add(entity);
                db.SaveChanges();

                //db.tbl_vendor.Add(model);
                //db.SaveChanges();


                ModelState.Clear();
                ViewBag.SuccessMessage = "Rgistration Successful!";
                return RedirectToAction("LoginVendor", "LoginVendor", new LoginViewModel());
            }
            return View("LoginVendor", new tbl_vendor());
        }

        [HttpGet]
        public ActionResult VendorProfile(tbl_vendor model)
        {   
            return View(model);
        }
        

        public ActionResult EditProfile()
        {
            
            return View();

        }


        [HttpPost]
        public ActionResult EditProfile(tbl_vendor model)
        {
            var user = (from v in db.tbl_vendor
                        where v.BIN.Equals(model.BIN)
                        select v).SingleOrDefault();
            if (model.Name != null && model.Email != null && model.Mobile != null && model.Address != null)
            {
                
                user.Name = model.Name;
                user.Email = model.Email;
                user.Mobile = model.Mobile;
                user.Address = model.Address;

                Session["NameSS"] = user.Name;

                db.SaveChanges();
            }

            return View("VendorProfile", user);
        }

        public ActionResult Edit()
        {
            return View() ;
        }

        public ActionResult DeleteVendor(tbl_vendor model)
        {
            var user = db.tbl_vendor.Where(x=>x.BIN == model.BIN).First();
            db.tbl_vendor.Remove(user);
            db.SaveChanges();


            Session["IdSS"] = null;
            Session["NameSS"] = null;
            return RedirectToAction("LoginVendor", "LoginVendor");
        }


        
    }
}