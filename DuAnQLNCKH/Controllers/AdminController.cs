using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class AdminController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        // GET: Admin
        public ActionResult Index(string seach)
        {

            Session["UserName"] = "admin";
            Session["Acess"] = "0";
            var model = qLNCKHDHTDTD.Accounts.ToList();
            ViewBag.listAccount = model;
            ViewBag.seaching = seach;
            return View();

        }
        public ActionResult Edit(string UserName)
        {

            Session["UserName"] = "admin";
            Session["Acess"] = "0";
            var model = qLNCKHDHTDTD.Accounts.Where(x=>x.UserName==UserName).SingleOrDefault();
            
            return View(model);

        }
        [HttpPost]
        public ActionResult EditAccount(Account account)
        {
 
            return View();

        }
        [HttpPost]
        public ActionResult CreateAccount(Account user)
        {
            Session["UserName"] = "admin";
            Session["Acess"] = "0";
            UserModel userModel = new UserModel();
            if (userModel.AddAccount(user))
                ViewBag.Message = "Success";
            var model = qLNCKHDHTDTD.Accounts.ToList();
            ViewBag.listAccount = model;
            return RedirectToAction("Index", "Admin");
        }
        public JsonResult DeleteAccount(string UserName)
        {
            bool a = qLNCKHDHTDTD.Database.ExecuteSqlCommand("delete from Account where UserName ='" + UserName + "'") > 0;

            return Json(new
            {
                UserName = UserName,
                a = a
            }, JsonRequestBehavior.AllowGet);
        }
    }
}