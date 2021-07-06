using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;
namespace DuAnQLNCKH.Controllers
{
    public class LoginController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        // GET: Login
        public ActionResult Index()
        {
            Session["Acess"] = "null";
            Session["UserName"] = "null";
            List<Account> acesslist = qLNCKHDHTDTD.Accounts.ToList();
            ViewBag.listacess = new SelectList(acesslist, "Access", "Access");
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login1(Account account)
        {
            if (ModelState.IsValid)
            {
                using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
                {
                    var obj = db.Accounts.Where(a => a.UserName.Equals(account.UserName) && a.PassWord.Equals(account.PassWord) && a.Access.Equals(account.Access)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Acess"] = obj.Access.ToString();
                        Session["UserName"] =  obj.UserName.ToString();
                        if (Session["Acess"].Equals("1"))
                        {
                            return RedirectToAction("Index","TopicOfLecture"); 
                        }
                        else if (Session["Acess"].Equals("0"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (Session["Acess"].Equals("2"))
                        return RedirectToAction("myTopicLecture", "TopicOfLecture");
                        return RedirectToAction("Index", "Notification");
                    }
                }
            }
            return View();
        }

        
    }
}