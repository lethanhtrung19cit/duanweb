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
        public ActionResult Index(string id)
        {
            Session["Acess"] = "null";
            Session["UserName"] = "null";
            Session["ID"] = id;
             return View();  
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login1(Account account)
        {
            
            var obj = qLNCKHDHTDTD.Accounts.Where(a => a.UserName.Equals(account.UserName) && a.PassWord.Equals(account.PassWord) && a.Access==account.Access).FirstOrDefault();
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
                {
                    return RedirectToAction("myTopicLecture", "TopicOfLecture");
                }    
                        
             }
                   
              
            ModelState.AddModelError("SessionLogin", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View("Index");
        }

        
    }
}