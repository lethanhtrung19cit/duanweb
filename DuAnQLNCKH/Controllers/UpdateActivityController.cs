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
        public void UpdateToLe(string IdTp, string Status, string Progress)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                 
                entities.Database.ExecuteSqlCommand("set dateformat dmy update TopicOfLecture set Progress=N'"+Status+"' where IdTp='" + IdTp + "' insert into ProgressLe(IdTp, Date, Status) values ('"+IdTp+"', '"+DateTime.Now.ToString("dd/MM/yyyy")+"', N'"+Progress+"')");
                entities.SaveChanges();
            }

        }
        public void UpdateToSt(string IdTp, string Status, string Progress)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {

                entities.Database.ExecuteSqlCommand("set dateformat dmy update TopicOfStudent set Progress=N'" + Status + "' where IdTp='" + IdTp + "' insert into ProgressSt(IdTp, Date, Status) values ('" + IdTp + "', '" + DateTime.Now.ToString("dd/MM/yyyy") + "', N'" + Progress + "')");
                entities.SaveChanges();
            }
        }
        public ActionResult Index()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                List<Extension> extensions = db.Extensions.ToList();
                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();
                List<ProgressLe> progressLes = db.ProgressLes.ToList();
                List<Information> information = db.Information.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();

                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join p in pointTables on t.IdP equals p.IdP
                                      join e in extensions on t.IdTp equals e.IdTp
                                      
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
            }
            return View();
        }
        public ActionResult ReportToSv()
        {
             using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<ProgressSt> progressSts = db.ProgressSts.ToList();
                List<PointTable> pointTables = db.PointTables.ToList();

                
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
            }
            return View();
        }
    }
}