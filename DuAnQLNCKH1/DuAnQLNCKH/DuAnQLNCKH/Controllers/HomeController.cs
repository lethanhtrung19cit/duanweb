﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;

namespace DuAnQLNCKH.Controllers
{
    public class HomeController : Controller
    {

      
        public ActionResult Index()
        {
            Session["Acess"] = "null";
            Session["UserName"] = "null";
            return View();
        }
        public ActionResult About()
        {
            

            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
        
    }
}