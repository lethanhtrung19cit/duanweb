using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class DetailNotificationController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        DetailNotificationModel dtnotify = new DetailNotificationModel();
        [HttpPost]
        public ActionResult Index(string IdNo)
        {

            var model = dtnotify.detailNotification(IdNo);
            return View(model);
            
        }
        
    }
}