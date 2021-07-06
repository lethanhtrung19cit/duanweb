using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;
using Type = DuAnQLNCKH.Models.Type;

namespace DuAnQLNCKH.Controllers
{
    public class TopicOfLectureController : Controller
    {
        // GET: DeTaiGV
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        TopicOfLectureModel dtgv = new TopicOfLectureModel();
        List<TopicOfStudent> studentList = new List<TopicOfStudent>();
        public ActionResult ListType()
        {
            List<Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View();
        }
        public ActionResult getTypeList(string IdTy)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = qLNCKHDHTDTD.PointTables.Where(x => x.IdTy == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                List<Extension> extensions = db.Extensions.ToList();
                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<Information> information = db.Information.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();
                List<Faculty> faculties = db.Faculties.ToList();
                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join p in pointTables on t.IdP equals p.IdP
                                      join e in extensions on t.IdTp equals e.IdTp
                                     
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfLecture = t,
                                          information = i,
                                          extension = e


                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                var topics = (from t in topicOfStudents
                              join p in pointTables on t.IdP equals p.IdP
                              join f in faculties on t.IdFa equals f.IdFa
                              where t.Status == "đã duyệt"
                              select new TopicOfStudentView
                              {

                                  topicOfStudent = t,
                                  faculty=f,
                                  pointTable = p

                              }).ToList();
                ViewBag.listTopicOfStudent = topics;
            }
          
            return View();

        }

        public ActionResult myTopicLecture()
        {
            

            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                List<Information> information = db.Information.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();
                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join p in pointTables on t.IdP equals p.IdP
                                      where i.UserName == Session["UserName"].ToString() 
                                      where t.Status=="chưa duyệt"                                     
                                      select new TopicOfLectureView
                                      {
                                          pointTable=p,
                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicExtension = topicofLecture;
                var topic = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join p in pointTables on t.IdP equals p.IdP
                                      where i.UserName == Session["UserName"].ToString()
                                      where t.Status == "đã duyệt"
                                      select new TopicOfLectureView
                                      {
                                          pointTable=p,
                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicProgress = topic;
                return View();
            }
        }
        //To Handle connection related activities
        [HttpPost]
        public void ShowIdP(string id)
        {
            ViewBag.idp = id;
            
        }
        public ActionResult CreateTopicOfLecture(TopicOfLecture topicOfLecture)
        {
            
            if (ModelState.IsValid)
            {
                string id = dtgv.IdTp();
                string username = Session["UserName"].ToString();
                TopicOfLectureModel topic = new TopicOfLectureModel();
                string idle = dtgv.IdLe(username);
                if (topic.AddTopicLecture(topicOfLecture, id, idle))
                    ViewBag.Message = "Employee details added successfully";
                
            }
            ModelState.Clear();
            
            List<Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View("ViewCreateTopicOfLecture");
           
        }
       
        public ActionResult ViewCreateTopicOfLecture()
        {
            List<Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View();
        }
       

        [HttpPost]   
        public JsonResult Delete(string IdTp)
        {
            bool a = qLNCKHDHTDTD.Database.ExecuteSqlCommand("delete from TopicOfLecture where IdTp='" + IdTp + "'")>0;

            return Json(new {
                IdTp = IdTp,
                a = a
            }, JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult chuaduyet()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<Information> information = db.Information.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();
                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join p in pointTables on t.IdP equals p.IdP
                                      
                                      where t.Status == "chưa duyệt"
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                var topicofStudent = (from t in topicOfStudents
                                     
                                      join p in pointTables on t.IdP equals p.IdP
                                      where t.Status == "chưa duyệt"
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfStudent = t
                                          
                                      }).ToList();
                ViewBag.TopicOfStudent = topicofStudent;
            }
           
            
            return View();
         
        }
        public ActionResult registerExtension(string IdTp, DateTime Times, string Reason)
        {
            
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                db.Database.ExecuteSqlCommand("update Extension set Times='" + Times + "', Reason=N'" + Reason + "', Status=N'chưa duyệt' where IdTp='"+IdTp+"'");
                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();
                List<Extension> extensions = db.Extensions.ToList();
                List<Information> information = db.Information.ToList();

                var topicextension = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join e in extensions on t.IdTp equals e.IdTp
                                      where i.UserName == Session["UserName"].ToString()

                                      select new TopicOfLectureView
                                      {
                                          extension = e,
                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicextension = topicextension;

            }
            return View("viewRegisterExtension");
        }
        public ActionResult viewRegisterExtension()
        {     
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();
                List<Extension> extensions = db.Extensions.ToList();
                List<Information> information = db.Information.ToList();

                var topicextension = (from t in topicOfLectures 
                                     join i in information on t.IdLe equals i.IdLe
                                     join e in extensions on t.IdTp equals e.IdTp
                                     where i.UserName== Session["UserName"].ToString()
                                     
                                      select new TopicOfLectureView
                                     {
                                         extension=e,
                                         topicOfLecture = t,
                                         information=i
                                     }).ToList();
                ViewBag.topicextension = topicextension;
                return View();
            }
        }
        public ActionResult listextension()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                List<Extension> extension = db.Extensions.ToList();
                
                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();
                List<Information> informations = db.Information.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();
                var topicextension =(from t in topicOfLectures 
                                     join e in extension on t.IdTp equals e.IdTp
                                     join i in informations on t.IdLe equals i.IdLe 
                                     join p in pointTables on t.IdP equals p.IdP
                                     where e.Status == "chưa duyệt"
                                     select new TopicOfLectureView
                                     {
                                         extension = e,
                                         topicOfLecture = t,
                                         information=i,
                                         pointTable=p
                                         
                                     }).ToList();
                ViewBag.listextension = topicextension;
                return View();
            }
          

        }
        
        public ActionResult chuaduyetsv()
        {            
            
             var model = dtgv.listchuaduyetsv();
            return View(model);
        }
        [HttpPost]
        [Route("/TopicOfLecture/extension")]
        public void extension(int IdEx, string NameTo, string Email)
        {
            
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                entities.Database.ExecuteSqlCommand("update Extension set Status = N'Đã duyệt' where IdEx=" + IdEx);
                entities.SaveChanges();
                string from1 = "trunglethanh6@gmail.com";
             
                // By using a Message with no constructors, you can define your To recipients below
                using (MailMessage mail = new MailMessage())
                {
                    // Define your senders
                    mail.From = new MailAddress(from1);
                    mail.Body = "Thông báo đề tài " + NameTo + " đã được gia hạn";
                    mail.Subject = "Thông báo gia hạn đề tài";

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
        public ActionResult updateExtension(string IdTp, string DateEx, string Reason)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                List<TopicOfLecture> topicOfLectures = entities.TopicOfLectures.ToList();
                List<Extension> extensions = entities.Extensions.ToList();
                List<Information> information = entities.Information.ToList();

                var topicextension = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join e in extensions on t.IdTp equals e.IdTp
                                      where i.UserName == Session["UserName"].ToString()

                                      select new TopicOfLectureView
                                      {
                                          extension = e,
                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicextension = topicextension;
                DateTime date = Convert.ToDateTime(DateEx);
                entities.Database.ExecuteSqlCommand("set dateformat dmy  update Extension set Times = '"+date+"', Reason=N'"+Reason+"' where IdTp='" + IdTp+"'");
                entities.SaveChanges();
            }
            return RedirectToAction("viewRegisterExtension");
        }
        [HttpPost]       
        public void xetduyet2(string IdTp, string Times, string NameTo, string Email)
        {

            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                string a = IdTp;
                DateTime Time = Convert.ToDateTime(Times);
                entities.Database.ExecuteSqlCommand("set dateformat dmy update TopicOfLecture set Status=N'đã duyệt', Progress=N'chờ báo cáo lần 1' where IdTp='" + IdTp + "' insert into Extension(IdTp, Times, Status) values('" + IdTp + "', '" + Time + "', 'NULL')");
                entities.SaveChanges();
                string from1 = "trunglethanh6@gmail.com";
                string s = Session["UserName"].ToString();

                // By using a Message with no constructors, you can define your To recipients below
                using (MailMessage mail = new MailMessage())
                {
                    // Define your senders
                    mail.From = new MailAddress(from1);
                    mail.Body = "Thông báo đề tài " + NameTo + " đã được xét duyệt";
                    mail.Subject = "Thông báo xét duyệt đề tài";
                    
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
        [HttpPost]
        
        public void xetduyetsv(TopicOfStudent topicOfStudent)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                TopicOfStudent topic = (from c in entities.TopicOfStudents
                                        where c.IdTp == topicOfStudent.IdTp
                                        select c).FirstOrDefault();
                entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã duyệt' where IdTp='" + topic.IdTp + "'");
                entities.SaveChanges();
            }
        }
        
    }
}