using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;

namespace DuAnQLNCKH.Controllers
{
    public class TopicOfStudentController : Controller
    {
        // GET: DeTaiGV
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        TopicOfStudentModel dtsv = new TopicOfStudentModel();
        public ActionResult Index()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();
                var topics = (from t in topicOfStudents
                              join p in pointTables on t.IdP equals p.IdP

                              where t.Status == "đã duyệt"
                              select new TopicOfStudentView
                              {

                                  topicOfStudent = t,

                                  pointTable = p

                              }).ToList();
                ViewBag.listTopicOfStudent = topics;
            }
            return View();

        }
        public ActionResult Register(TopicOfStudent topicOfStudent)
        {
            if (ModelState.IsValid)
            {
                string id = dtsv.IdTp();

                TopicOfStudentModel topic = new TopicOfStudentModel();
                if (topic.AddTopicStudent(topicOfStudent, id))
                    ViewBag.Message = "Employee details added successfully";

            }
            ModelState.Clear();

            List<Models.Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            List<Faculty> faculties = qLNCKHDHTDTD.Faculties.ToList();
            ViewBag.listFaculty = new SelectList(faculties, "IdFa", "Name");
            return View("viewRegister");
        }
      
        public ActionResult getTypeList(string IdTy)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = qLNCKHDHTDTD.PointTables.Where(x => x.IdTy == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult viewRegister()
        {
            List<Models.Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            List<Faculty> faculties = qLNCKHDHTDTD.Faculties.ToList();
            ViewBag.listFaculty = new SelectList(faculties, "IdFa", "Name");
            return View();
        }
       
        [HttpPost]
        public JsonResult Delete(string IdTp)
        {
            bool a = qLNCKHDHTDTD.Database.ExecuteSqlCommand("delete from TopicOfStudent where IdTp='" + IdTp + "'") > 0;

            return Json(new
            {
                IdTp = IdTp,
                a = a
            }, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult chuaduyet()
        {
            
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();
                var topics = (from t in topicOfStudents
                                      join p in pointTables on t.IdP equals p.IdP
                                     
                                      where t.Status == "chưa duyệt"
                                      select new TopicOfStudentView
                                      {
                                          
                                          topicOfStudent = t,
                                          
                                          pointTable = p

                                      }).ToList();
                ViewBag.listextension = topics;
            }
            return View();
        }


        [HttpPost]
        [Route("/TopicOfStudent/xetduyetsv")]
        public void xetduyetsv(string IdTp,string NameTo, string Email)
        {


            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã duyệt', Progress=N'đang thực hiện' where IdTp='" + IdTp + "'");
                entities.SaveChanges();
                string from1 = "trunglethanh6@gmail.com";
                string s = Session["UserName"].ToString();

                // By using a Message with no constructors, you can define your To recipients below
                using (MailMessage mail = new MailMessage())
                {
                    // Define your senders
                    mail.From = new MailAddress(from1);
                    mail.Body = "Thông báo đề tài " + NameTo + " đã được xét duyệt";
                    mail.Subject = "Thông báo sinh viên xét duyệt đề tài";

                    mail.To.Add(Email);

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from1, "lethanhtrung18082001");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                }
            }

        }       
    }
}