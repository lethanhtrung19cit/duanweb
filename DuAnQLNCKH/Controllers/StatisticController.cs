using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DuAnQLNCKH.Models;


namespace DuAnQLNCKH.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        DHTDTTDNEntities1 dHTDTTDNEntities1 = new DHTDTTDNEntities1();
       
        public ActionResult Index()
        {
            Session["UserName"] = "pnckh";
            Session["Acess"] = "1";
            viewbag();
           
            return View();
        }
        
        public ActionResult journal()
        {

            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                List<PointTable> pointTables = db.PointTables.ToList();

                List<Information> information = db.Information.ToList();
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<Models.Type> types = db.Types.ToList();
                var topicOfLecture = (from t in topicOfLectures
                                      join p in pointTables on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy
                                      join i in information on t.IdLe equals i.IdLe
                                      
                                      where t.Status == "đã duyệt" && ty.Name=="Tạp chí"
                                      select new TopicOfLectureView
                                      {
                                          pointTable = p,
                                          topicOfLecture = t,
                                          information = i,
                                          type = ty
                                          
                                      }).ToList();
                ViewBag.listTopicOfLecture = topicOfLecture;
                var topicOfStudent = (from t in topicOfStudents
                                      join p in pointTables on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy

                                      where t.Status == "đã duyệt" && ty.Name == "Tạp chí"
                                      select new TopicOfStudentView
                                      {
                                          pointTable = p,
                                          topicOfStudent = t,
                                          type=ty

                                      }).ToList();
                ViewBag.topicOfStudents = topicOfStudent;
                var listJournal = (from t in types
                                   join p in pointTables on t.IdTy equals p.IdTy
                                   where t.Name == "Tạp chí"
                                   select new
                                   {
                                       p.NameP,
                                       p.IdP

                                   }).ToList();
                ViewBag.listJournal = new SelectList(listJournal, "IdP", "NameP");
            }

           

            return View();
        }
         
        public void viewbag()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                List<PointTable> pointTables = db.PointTables.ToList();
                List<Faculty> faculties = db.Faculties.ToList();
                List<Information> information = db.Information.ToList();
                List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
                List<Models.Type> types = db.Types.ToList();
                var topicOfLecture = (from t in topicOfLectures
                                      join p in pointTables  on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy
                                      join i in information on t.IdLe equals i.IdLe
                                      join f in faculties on i.IdKhoa equals f.IdFa
                                      where t.Status=="đã duyệt"
                                      select new TopicOfLectureView
                                      {
                                          type=ty,
                                          pointTable=p,
                                          topicOfLecture = t,
                                          information = i,
                                          faculty = f
                                      }).ToList();
                ViewBag.listTopicOfLecture = topicOfLecture;
                var topicOfStudent1 = (from t in topicOfStudents
                                       
                                       join p in pointTables on t.IdP equals p.IdP
                                       join ty in types on p.IdTy equals ty.IdTy 
                                       join f in faculties on t.IdFa equals f.IdFa
                                       where t.Status == "đã duyệt"
                                       select new TopicOfStudentView
                                       {

                                           topicOfStudent = t,
                                           faculty = f,
                                           type=ty,
                                           pointTable = p

                                       }).ToList();
                ViewBag.listtopicOfStudents = topicOfStudent1;
                var listJournal = (from t in types
                                   join p in pointTables on t.IdTy equals p.IdTy
                                   where t.Name == "Tạp chí"
                                   select new 
                                   {
                                      p.NameP,
                                      p.IdP

                                   }).ToList();
                ViewBag.listJournal = new SelectList(listJournal, "IdP", "NameP");
                ViewBag.listNameStu = new SelectList(topicOfStudents, "IdSV", "NameSt");
            }
            
            List<Models.Type> typelist = dHTDTTDNEntities1.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            List<Faculty> faculties1 = dHTDTTDNEntities1.Faculties.ToList();
            ViewBag.listFacul = new SelectList(faculties1, "IdFa", "Name");
            List<Information> information1 = dHTDTTDNEntities1.Information.ToList();
            ViewBag.listNameLe = new SelectList(information1, "IdLe", "NameLe");
            
        }

        public ActionResult getTypeList(string IdTy)
        {
            dHTDTTDNEntities1.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = dHTDTTDNEntities1.PointTables.Where(x => x.IdTy == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchLecture(string name)
        {
            List<TopicOfLecture> listTopicOfLecture = dHTDTTDNEntities1.TopicOfLectures.Where(x=>x.Name.Contains(name)).ToList();
            List<TopicOfStudent> listTopicOfStudent = dHTDTTDNEntities1.TopicOfStudents.ToList();
            ViewBag.listTopicOfStudent = listTopicOfStudent;
            ViewBag.listTopicOfLecture = listTopicOfLecture;
            return View("Index");
        }
        [HttpGet]
        public JsonResult LoadData(string name)
        {
            IQueryable<TopicOfLecture> model = dHTDTTDNEntities1.TopicOfLectures;

            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.Name.Contains(name));
 
            int totalRow = model.Count();
            return Json(new
            {
                data = model,
                total = totalRow
                
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Journal()
        //{
        //    List<Models.Type> typelist = dHTDTTDNEntities1.Types.ToList();
        //    ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
        //    return View();
        //}
        private SqlConnection con;
        public string IdP1;
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        [HttpPost]
        public ActionResult ExportExcel(string IdTya, string IdFa, string IdLea, DateTime dateEnd, DateTime dateSt)
        {
            var gv = new GridView();
            //ViewBag.IdP = "22";
            //string dateEnd1 = dateEnd.ToString("dd/MM/yyyy");
            //string dateSt1 = dateSt.ToString("dd/MM/yyyy");
            
            string daSt = dateSt.Date.ToString();
            
            string daEn = dateEnd.Date.ToString();
            //DateTime dateSt1 = new DateTime(dateSt);
             
             
                
            List<TopicOfLecture> topicOfLectures = dHTDTTDNEntities1.TopicOfLectures.ToList();
            List<Models.Type> types = dHTDTTDNEntities1.Types.ToList();
            List<Information> information = dHTDTTDNEntities1.Information.ToList();
            List<PointTable> pointTables = dHTDTTDNEntities1.PointTables.ToList();
            List<Faculty> faculties = dHTDTTDNEntities1.Faculties.ToList();
            TopicOfLecture to = new TopicOfLecture();
            if (IdLea != "")
            {
                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join f in faculties on i.IdKhoa equals f.IdFa
                                      join p in pointTables on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy

                                      where
                                      IdTya != "" ?
                                                  (IdFa != "" ?
                                                              (IdLea != "" ? ty.IdTy == IdTya && t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && i.IdLe == IdLea && t.Status == "đã duyệt"
                                                                             : ty.IdTy == IdTya && t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && t.Status == "đã duyệt")
                                                              : (IdLea != "" ? ty.IdTy == IdTya && i.IdLe == IdLea && t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"
                                                                              : ty.IdTy == IdTya && t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"))
                                                   : (IdFa != "" ?
                                                                 (IdLea != "" ? t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && i.IdLe == IdLea && t.Status == "đã duyệt"
                                                                              : t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && t.Status == "đã duyệt")
                                                                 : (IdLea != "" ? i.IdLe == IdLea && t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"
                                                                                : t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"))

                                      select new
                                      {
                                          t.Name,
                                          i.NameLe,
                                          NameKhoa = f.Name,
                                          NameType = ty.Name,
                                          t.DateSt,
                                          t.Times,
                                          DateEnd = t.DateSt.AddMonths(t.Times).ToString("dd/MM/yyyy"),

                                          t.Expense,
                                          t.Progress,

                                          Value = p.Value / t.CountAuthor
                                      }).ToList();
                gv.DataSource = topicofLecture;

            }
            else
            {
                var topicofLecture = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      join f in faculties on i.IdKhoa equals f.IdFa
                                      join p in pointTables on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy

                                      where
                                      IdTya != "" ?
                                                  (IdFa != "" ?
                                                              (IdLea != "" ? ty.IdTy == IdTya && t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && i.IdLe == IdLea && t.Status == "đã duyệt"
                                                                             : ty.IdTy == IdTya && t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && t.Status == "đã duyệt")
                                                              : (IdLea != "" ? ty.IdTy == IdTya && i.IdLe == IdLea && t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"
                                                                              : ty.IdTy == IdTya && t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"))
                                                   : (IdFa != "" ?
                                                                 (IdLea != "" ? t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && i.IdLe == IdLea && t.Status == "đã duyệt"
                                                                              : t.DateSt >= dateSt && t.DateSt <= dateEnd && f.IdFa == IdFa && t.Status == "đã duyệt")
                                                                 : (IdLea != "" ? i.IdLe == IdLea && t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"
                                                                                : t.DateSt >= dateSt && t.DateSt <= dateEnd && t.Status == "đã duyệt"))

                                      select new
                                      {
                                          t.Name,
                                          i.NameLe,
                                          NameKhoa = f.Name,
                                          NameType = ty.Name,
                                          t.DateSt,
                                          t.Times,
                                          DateEnd = t.DateSt.AddMonths(t.Times).ToString("dd/MM/yyyy"),
                                          t.Expense,
                                          t.Progress,


                                      }).ToList();
                gv.DataSource = topicofLecture;
            }
            
            
            
            //gv.DataSource = dHTDTTDNEntities1.TopicOfLectures.Where(a => a.IdP == a1 && a.DateSt >= dateSt && a.DateSt<= dateEnd).Select(x=>x.Name)
            gv.DataBind();
            for (int i=0;i<gv.Rows.Count;i++)
            {
                //var r2 = gv.Rows[i].Cells[3].Text;
                //var r1 = Convert.ToDateTime(gv.Rows[i].Cells[3].Text).ToString("dd/MM/yyyy");
                gv.Rows[i].Cells[4].Text = Convert.ToDateTime(gv.Rows[i].Cells[4].Text).ToString("dd/MM/yyyy");
                gv.Rows[i].Cells[5].Text = gv.Rows[i].Cells[5].Text + " tháng";
                gv.Rows[i].Cells[7].Text = gv.Rows[i].Cells[7].Text + " VND";
            }
            gv.HeaderRow.Cells[0].Text = "Tên đề tài";
            gv.HeaderRow.Cells[1].Text = "Tên giảng viên";
            gv.HeaderRow.Cells[2].Text = "Tên khoa";
            gv.HeaderRow.Cells[3].Text = "Loại đề tài";
            gv.HeaderRow.Cells[4].Text = "Ngày bắt đầu";
            gv.HeaderRow.Cells[5].Text = "Thời gian";
            gv.HeaderRow.Cells[6].Text = "Kết thúc";
            gv.HeaderRow.Cells[7].Text = "Kinh phí";
            gv.HeaderRow.Cells[8].Text = "Trạng thái";
            if (IdLea!="")
            {             
                gv.HeaderRow.Cells[9].Text = "Điểm";
            }    
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            viewbag();
            return View("Index");
        }
        [HttpPost]
        public ActionResult ExportExcelStu(string IdTyaStu, string IdFaStu, string IdLeaStu, DateTime dateEndStu, DateTime dateStStu)
        {
            var gv = new GridView();
            //ViewBag.IdP = "22";
            //string dateEnd1 = dateEnd.ToString("dd/MM/yyyy");
            //string dateSt1 = dateSt.ToString("dd/MM/yyyy");

            string daSt = dateStStu.Date.ToString();

            string daEn = dateEndStu.Date.ToString();
            //DateTime dateSt1 = new DateTime(dateSt);



            List<TopicOfStudent> topicOfLectures = dHTDTTDNEntities1.TopicOfStudents.ToList();
            List<Models.Type> types = dHTDTTDNEntities1.Types.ToList();
             List<PointTable> pointTables = dHTDTTDNEntities1.PointTables.ToList();
            List<Faculty> faculties = dHTDTTDNEntities1.Faculties.ToList();
            TopicOfLecture to = new TopicOfLecture();
            if (IdLeaStu != "")
            {
                var topicofLecture = (from t in topicOfLectures
                                       join f in faculties on t.IdFa equals f.IdFa
                                      join p in pointTables on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy

                                      where
                                      IdTyaStu != "" ?
                                                  (IdFaStu != "" ?
                                                              (IdLeaStu != "" ? ty.IdTy == IdTyaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.IdSV == IdLeaStu && t.Status == "đã duyệt"
                                                                             : ty.IdTy == IdTyaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.Status == "đã duyệt")
                                                              : (IdLeaStu != "" ? ty.IdTy == IdTyaStu && t.IdSV == IdLeaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"
                                                                              : ty.IdTy == IdTyaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"))
                                                   : (IdFaStu != "" ?
                                                                 (IdLeaStu != "" ? t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.IdSV == IdLeaStu && t.Status == "đã duyệt"
                                                                              : t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.Status == "đã duyệt")
                                                                 : (IdLeaStu != "" ? t.IdSV == IdLeaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"
                                                                                : t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"))

                                      select new
                                      {
                                          t.Name,
                                          t.NameSt,
                                          NameKhoa = f.Name,
                                          NameType = ty.Name,
                                          t.DateSt,
                                          t.Times,
                                          DateEnd = t.DateSt.AddMonths(t.Times).ToString("dd/MM/yyyy"),
                                          t.Expense,
                                          t.Progress,

                                          Value = p.Value / t.CountAuthor
                                      }).ToList();
                gv.DataSource = topicofLecture;

            }
            else
            {
                var topicofLecture = (from t in topicOfLectures
                                       join f in faculties on t.IdSV equals f.IdFa
                                      join p in pointTables on t.IdP equals p.IdP
                                      join ty in types on p.IdTy equals ty.IdTy

                                      where
                                      IdTyaStu != "" ?
                                                  (IdFaStu != "" ?
                                                              (IdLeaStu != "" ? ty.IdTy == IdTyaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.IdSV == IdLeaStu && t.Status == "đã duyệt"
                                                                             : ty.IdTy == IdTyaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.Status == "đã duyệt")
                                                              : (IdLeaStu != "" ? ty.IdTy == IdTyaStu && t.IdSV == IdLeaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"
                                                                              : ty.IdTy == IdTyaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"))
                                                   : (IdFaStu != "" ?
                                                                 (IdLeaStu != "" ? t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.IdSV == IdLeaStu && t.Status == "đã duyệt"
                                                                              : t.DateSt >= dateStStu && t.DateSt <= dateEndStu && f.IdFa == IdFaStu && t.Status == "đã duyệt")
                                                                 : (IdLeaStu != "" ? t.IdSV == IdLeaStu && t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"
                                                                                : t.DateSt >= dateStStu && t.DateSt <= dateEndStu && t.Status == "đã duyệt"))

                                      select new
                                      {
                                          t.Name,
                                          t.NameSt,
                                          NameKhoa = f.Name,
                                          NameType = ty.Name,
                                          t.DateSt,
                                          t.Times,
                                          DateEnd = t.DateSt.AddMonths(t.Times).ToString("dd/MM/yyyy"),

                                          t.Expense,
                                          t.Progress,


                                      }).ToList();
                gv.DataSource = topicofLecture;
            }



            //gv.DataSource = dHTDTTDNEntities1.TopicOfLectures.Where(a => a.IdP == a1 && a.DateSt >= dateSt && a.DateSt<= dateEnd).Select(x=>x.Name)
            gv.DataBind();
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                //var r2 = gv.Rows[i].Cells[3].Text;
                //var r1 = Convert.ToDateTime(gv.Rows[i].Cells[3].Text).ToString("dd/MM/yyyy");
                gv.Rows[i].Cells[4].Text = Convert.ToDateTime(gv.Rows[i].Cells[4].Text).ToString("dd/MM/yyyy");
                gv.Rows[i].Cells[5].Text = gv.Rows[i].Cells[5].Text + " tháng";
                gv.Rows[i].Cells[7].Text = gv.Rows[i].Cells[7].Text + " VND";
            }
            gv.HeaderRow.Cells[0].Text = "Tên đề tài";
            gv.HeaderRow.Cells[1].Text = "Tên sinh viên";
            gv.HeaderRow.Cells[2].Text = "Tên khoa";
            gv.HeaderRow.Cells[3].Text = "Loại đề tài";
            gv.HeaderRow.Cells[4].Text = "Ngày bắt đầu";
            gv.HeaderRow.Cells[5].Text = "Thời gian";
            gv.HeaderRow.Cells[6].Text = "Kết thúc";
            gv.HeaderRow.Cells[7].Text = "Kinh phí";
            gv.HeaderRow.Cells[8].Text = "Trạng thái";
            if (IdLeaStu != "")
            {
                gv.HeaderRow.Cells[9].Text = "Điểm";
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            viewbag();
            return View("Index");
        }

        [HttpPost]
        
        public JsonResult ExportToExcel(string IdPa, DateTime dateEnd, DateTime dateSt)
        {
            ViewBag.IdP = IdPa;
            IdP1 = IdPa;
            viewbag();
            return Json("Index");
        }
        public string idp()
        {

            return ViewBag.IdP;
        }
    }
}