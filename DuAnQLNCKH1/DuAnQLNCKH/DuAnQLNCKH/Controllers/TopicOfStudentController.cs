﻿using System;
using System.Collections.Generic;
using System.IO;
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
             
                List<TopicOfStudent> topicOfStudents = qLNCKHDHTDTD.TopicOfStudents.ToList();
                List<PointTable> pointTables = qLNCKHDHTDTD.PointTables.ToList();
                List<Faculty> faculties = qLNCKHDHTDTD.Faculties.ToList();
                var topics = (from t in topicOfStudents
                          join p in pointTables on t.IdP equals p.IdP
                          join f in faculties on t.IdFa equals f.IdFa
                          where t.Status == "chưa duyệt"
                          select new TopicOfStudentView
                          {

                              topicOfStudent = t,
                              faculty=f,
                              pointTable = p

                          }).OrderBy(x => x.topicOfStudent.DateSt).ToList();
                ViewBag.topicPending = topics;
                var topicProgress = (from t in topicOfStudents
                          join p in pointTables on t.IdP equals p.IdP
                          join f in faculties on t.IdFa equals f.IdFa
                          where t.Status == "đã duyệt"
                          select new TopicOfStudentView
                          {

                              topicOfStudent = t,
                              faculty = f,
                              pointTable = p

                          }).OrderBy(x => x.topicOfStudent.DateSt).ToList();
                ViewBag.topicProgress = topicProgress;
                 var topicEnd = (from t in topicOfStudents
                          join p in pointTables on t.IdP equals p.IdP
                          join f in faculties on t.IdFa equals f.IdFa
                          where t.Status == "đã nghiệm thu"
                          select new TopicOfStudentView
                          {

                              topicOfStudent = t,
                              faculty=f,
                              pointTable = p

                          }).OrderBy(x => x.topicOfStudent.DateSt).ToList();
                ViewBag.topicEnd = topicEnd;
                
            
            return View();

        }
        public ActionResult Register(HttpPostedFileBase FileDemo1, TopicOfStudent topicOfStudent)
        {
            if (ModelState.IsValid)
            {
                string id = dtsv.IdTp();

                TopicOfStudentModel topic = new TopicOfStudentModel();
                if (FileDemo1 != null)
                {
                    var Extension = Path.GetExtension(FileDemo1.FileName);
                    var fileName = "fileTopic-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                    string path = Path.Combine(Server.MapPath("~/File/FileTopic"), fileName);
                    topicOfStudent.FileDemo = Url.Content(Path.Combine("~/File/FileTopic/", fileName));
                    if (topic.AddTopicStudent(topicOfStudent, id))
                    {
                        FileDemo1.SaveAs(path);
                        ViewBag.Message = "Employee details added successfully";
                        List<Models.Type> typelist1 = qLNCKHDHTDTD.Types.ToList();
                        ViewBag.listtype = new SelectList(typelist1, "IdTy", "Name");
                        List<Faculty> faculties1 = qLNCKHDHTDTD.Faculties.ToList();
                        ViewBag.listFaculty = new SelectList(faculties1, "IdFa", "Name");
                        return View("viewRegister");
                    }    
                }
                else
                {
                  
                    qLNCKHDHTDTD.Database.ExecuteSqlCommand("set dateformat dmy Insert into TopicOfStudent(IdTp, Name, NameSt, IdSV, IdP, DateSt, Times, Expense, Status, Progress, CountAuthor, IdFa) values ('" + id + "', N'" + topicOfStudent.Name + "', N'" + topicOfStudent.NameSt + "', '" + id + "', " + "', '" + topicOfStudent.IdSV + topicOfStudent.IdP + ", '" + topicOfStudent.DateSt + "', " + topicOfStudent.Times + ", " + topicOfStudent.Expense + ", N'chưa duyệt', N'chờ duyệt', " + topicOfStudent.CountAuthor + "', '" + topicOfStudent.IdFa + "')");
                    ViewBag.Message = "Employee details added successfully";
                    List<Models.Type> typelist1 = qLNCKHDHTDTD.Types.ToList();
                    ViewBag.listtype = new SelectList(typelist1, "IdTy", "Name");
                    List<Faculty> faculties1 = qLNCKHDHTDTD.Faculties.ToList();
                    ViewBag.listFaculty = new SelectList(faculties1, "IdFa", "Name");
                    return View("viewRegister");
                }
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