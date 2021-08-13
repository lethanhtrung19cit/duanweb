using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class ReportController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        ReportStatementModel report = new ReportStatementModel();
        // GET: Report
        public ActionResult Index()
        {

            ViewBag.listStatement = qLNCKHDHTDTD.Statements.ToList();
            return View();

        }
        public ActionResult AddStatement(Statement statement)
        {
            if (ModelState.IsValid)
            {
             
                ReportStatementModel report = new ReportStatementModel();
                if (report.AddStatement(statement))
                    ViewBag.Message = "Employee details added successfully";

            }
            ModelState.Clear();
            ViewBag.listStatement = qLNCKHDHTDTD.Statements.ToList();
            return View("Index");
        } 
    }
}