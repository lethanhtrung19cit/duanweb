using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class UpdateActivityController : Controller
    {
        // GET: UpdateActivity
        DHTDTTDNEntities1 entities = new DHTDTTDNEntities1();
        public void UpdateToLe(string IdTp, string Status)
        {
             
                 
                entities.Database.ExecuteSqlCommand("set dateformat dmy update TopicOfLecture set Progress=N'"+Status+"' where IdTp='" + IdTp + "' insert into ProgressLe(IdTp, Date, Status) values ('"+IdTp+"', '"+DateTime.Now.ToString("dd/MM/yyyy")+"', N'"+Status+"')");
                entities.SaveChanges();
             

        }
        public void UpdateToSt(string IdTp, string Status)
        {
            

                entities.Database.ExecuteSqlCommand("set dateformat dmy update TopicOfStudent set Progress=N'" + Status + "' where IdTp='" + IdTp + "' insert into ProgressSt(IdTp, Date, Status) values ('" + IdTp + "', '" + DateTime.Now.ToString("dd/MM/yyyy") + "', N'" + Status + "')");
                entities.SaveChanges();
             
        }
        public ActionResult Index()
        {
             
                List<Extension> extensions = entities.Extensions.ToList();
                List<TopicOfLecture> topicOfLectures = entities.TopicOfLectures.ToList();
                List<ProgressLe> progressLes = entities.ProgressLes.ToList();
                List<Information> information = entities.Information.ToList();
                List<PointTable> pointTables = entities.PointTables.ToList();

                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join p in pointTables on t.IdP equals p.IdP
                                      join e in extensions on t.IdTp equals e.IdTp
                                      where i.UserName==Session["UserName"].ToString()
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfLecture = t,
                                          information = i,
                                          extension = e,
                                         


                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;

                var detailProgress = (from t in topicOfLectures
                                    
                                    join pr in progressLes on t.IdTp equals pr.IdTp
                                    
                                    select new TopicOfLectureView
                                    {
                                        
                                        topicOfLecture = t,
                                        progressLe=pr
 
                                    }).ToList();
                ViewBag.detailProgress = detailProgress;
             
            return View();
        }
        public ActionResult UpdateAcceptance()
        {
            

                List<TopicOfLecture> topicOfLectures = entities.TopicOfLectures.ToList();
                List<TopicOfStudent> topicOfStudents = entities.TopicOfStudents.ToList();
                List<Information> information = entities.Information.ToList();
                List<PointTable> pointTables = entities.PointTables.ToList();
                List<Faculty> faculties = entities.Faculties.ToList();
                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join f in faculties on i.IdKhoa equals f.IdFa
                                      join p in pointTables on t.IdP equals p.IdP
                                      where t.Status == "đã duyệt"
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfLecture = t,
                                          information = i,
                                          faculty = f
                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                var topicofStudent = (from t in topicOfStudents
                                      join f in faculties on t.IdFa equals f.IdFa
                                      join p in pointTables on t.IdP equals p.IdP
                                      where t.Status == "đã duyệt"
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfStudent = t,
                                          faculty = f
                                      }).ToList();
                ViewBag.TopicOfStudent = topicofStudent;
            
            return View();
        }
        public void Acceptance(string IdTp)
        {
            entities.Database.ExecuteSqlCommand("update TopicOfLecture set Status=N'đã nghiệm thu' where IdTp='" + IdTp + "'");
            entities.SaveChanges();
         }
        public void AcceptanceStu(string IdTp)
        {
            entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã nghiệm thu' where IdTp='" + IdTp + "'");
            entities.SaveChanges();
         }
        public ActionResult ReportToSv()
        {
             
                
                List<TopicOfStudent> topicOfStudents = entities.TopicOfStudents.ToList();
                List<ProgressSt> progressSts = entities.ProgressSts.ToList();
                List<PointTable> pointTables = entities.PointTables.ToList();

                
                var topics = (from t in topicOfStudents
                              join p in pointTables on t.IdP equals p.IdP
                               
                              where t.Status == "đã duyệt"
                              select new TopicOfStudentView
                              {

                                  topicOfStudent = t,                                  
                                  pointTable = p,
                                  

                              }).ToList();
                ViewBag.listTopicOfStudent = topics;
                var detailProgress = (from t in topicOfStudents

                                      join pr in progressSts on t.IdTp equals pr.IdTp

                                      select new TopicOfStudentView
                                      {

                                          topicOfStudent = t,
                                          progressSt = pr

                                      }).ToList();
                ViewBag.detailProgressSt = detailProgress;
            
            return View();
        }
        
    }
}