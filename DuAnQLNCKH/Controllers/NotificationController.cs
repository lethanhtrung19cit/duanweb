using DuAnQLNCKH.Models;
using Microsoft.SharePoint.Client;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Attachment = System.Net.Mail.Attachment;

namespace DuAnQLNCKH.Controllers
{
    public class NotificationController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        NotificationModel notify = new NotificationModel();
        private SqlConnection con;
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        public ActionResult sendEmail(string Body, string Subject, string Progress, selectedEmail emp)
        {
            viewbag();
            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName = c.FileName
                         }).ToList();
            //var list1 = demo.Database.SqlQuery<tblFileDetail>("select * from tblFileDetails").ToList();

            // model.ListFile = list1;
            ViewBag.listNoti = list1;
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).ToList();

            string from1 = "trunglethanh6@gmail.com";
            

            // By using a Message with no constructors, you can define your To recipients below
            using (MailMessage mail = new MailMessage())
            {
                // Define your senders
                mail.From = new MailAddress(from1);
                mail.Body = Body;
                mail.Subject = Subject;
                // Add your multiple addresses to the To collection (you could use a loop for this as well)
                
                qLNCKHDHTDTD.SaveChanges();
                List<Information> information1 = new List<Information>();
               foreach (var i in emp.SelectedIdArray)
                {
                    mail.To.Add(i);

                }
               
                //List<TopicOfLecture> topicOfLectures = qLNCKHDHTDTD.TopicOfLectures.ToList();

                //List<Information> information = qLNCKHDHTDTD.Information.ToList();
                //List<PointTable> pointTables = qLNCKHDHTDTD.PointTables.ToList();
                //TopicOfLecture to = new TopicOfLecture();
                //var topicofLecture = (from t in topicOfLectures
                //                      join i in information on t.IdLe equals i.IdLe
                //                      join p in pointTables on t.IdP equals p.IdP
                //                      where t.Progress == Progress
                //                      select new 
                //                      {
                //                          t.Name,
                //                          i.NameLe,
                //                         p.NameP,
                //                         t.DateSt,
                //                         t.Status,
                //                         t.Progress
                //                      }).ToList();

                
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
               
                //GridView dgGrid = new GridView();

               
                //dgGrid.DataSource = topicofLecture;
               
                //dgGrid.DataBind();

                //for (int i = 0; i < dgGrid.Rows.Count; i++)
                //{
                //    //var r2 = gv.Rows[i].Cells[3].Text;
                //    //var r1 = Convert.ToDateTime(gv.Rows[i].Cells[3].Text).ToString("dd/MM/yyyy");
                //    dgGrid.Rows[i].Cells[3].Text = Convert.ToDateTime(dgGrid.Rows[i].Cells[3].Text).ToString("dd/MM/yyyy");


                //}


                //dgGrid.HeaderRow.Cells[0].Text = "Tên đề tài";
                //dgGrid.HeaderRow.Cells[1].Text = "Tên giảng viên";
                //dgGrid.HeaderRow.Cells[2].Text = "Loại đề tài";
                //dgGrid.HeaderRow.Cells[3].Text = "Ngày bắt đầu";
                //dgGrid.HeaderRow.Cells[4].Text = "Trạng thái";
                //dgGrid.HeaderRow.Cells[5].Text = "Tiến độ";


                
                //dgGrid.HeaderStyle.Font.Bold = true;
                ////Get the HTML for the control.
                
                //dgGrid.RenderControl(hw);
                


                ////Write the HTML back to the browser.
                ////Response.ContentType = application/vnd.ms-excel;
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentEncoding = System.Text.Encoding.Default;
               
                ////}
                //System.IO.MemoryStream s = new MemoryStream();
                //System.Text.Encoding Enc = System.Text.Encoding.UTF8;
                //byte[] mBArray = Enc.GetBytes(tw.ToString());
                //s = new MemoryStream(mBArray, false);
                //Attachment at = new Attachment(s, "ThongBaoGiangVien.xls");

                //mail.Attachments.Add(at);
                
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from1, "lethanhtrung18082001");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
               

                return View("Index");
            }
            
            
           
        }
       // public string[] SelectedIdArray { get; set; }

        //send email excel
        public void viewbag()
        {
            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName = c.FileName
                         }).ToList();
            //var list1 = demo.Database.SqlQuery<tblFileDetail>("select * from tblFileDetails").ToList();

             
            ViewBag.listNoti = list1;
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).ToList();
            using (DHTDTTDNEntities1 a = new DHTDTTDNEntities1())
            {
                //List<TopicOfLecture> listProgress = a.TopicOfLectures.SqlQuery("select distinct Progress=CAST(Progress AS NVARCHAR(MAX)) from TopicOfLecture").ToList<TopicOfLecture>();           
                //List<TopicOfLecture> listProgress = a.TopicOfLectures.ToList();
                List<TopicOfLecture> listProgress = a.TopicOfLectures.ToList();


                ViewBag.listProgress = new MultiSelectList(a.TopicOfLectures.Select(x => x.Progress).Distinct().ToList(), "Progress");
                //ema.EmailCollection = a.Information.ToList();
                ViewBag.listEmail = a.Information.ToList();

            }
           
        }
        public ActionResult Index(FileNotifiModel model)
        {
            selectedEmail ema = new selectedEmail();

            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName=c.FileName
                         }).ToList();
            //var list1 = demo.Database.SqlQuery<tblFileDetail>("select * from tblFileDetails").ToList();
           
            model.ListFile = list1;
            ViewBag.listNoti = list1;
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).ToList();
            using (DHTDTTDNEntities1 a = new DHTDTTDNEntities1())
            {
                //List<TopicOfLecture> listProgress = a.TopicOfLectures.SqlQuery("select distinct Progress=CAST(Progress AS NVARCHAR(MAX)) from TopicOfLecture").ToList<TopicOfLecture>();           
                //List<TopicOfLecture> listProgress = a.TopicOfLectures.ToList();
                List<TopicOfLecture> listProgress = a.TopicOfLectures.ToList();
              

                ViewBag.listProgress = new MultiSelectList(a.TopicOfLectures.Select(x=>x.Progress).Distinct().ToList(), "Progress");
                ema.EmailCollection = a.Information.ToList();
                
            }
            ViewBag.listEmail = ema.EmailCollection;
            //var listProgress = qLNCKHDHTDTD.TopicOfLectures.Select(x => Convert(NVar)).Distinct().ToList();

            return View();
        }
        public ActionResult getTypeList(string Progress)
        {
            List<TopicOfLecture> topicOfLectures = qLNCKHDHTDTD.TopicOfLectures.ToList();
 
            List<Information> informations = qLNCKHDHTDTD.Information.ToList();
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            var listJournal = (from t in topicOfLectures
                               join i in informations on t.IdLe equals i.IdLe
                               where t.Progress == Progress
                               select new 
                               {
                                  i.IdLe,
                                  i.Email

                               }).ToList();
          
            return Json(listJournal, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateNotifi(HttpPostedFileBase files, FileNotifiModel model)
        {

            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName = c.FileName
                         }).ToList();

            model.ListFile = list1;
            ViewBag.FileList = list1;

            if (files != null)
            {
                var Extension = Path.GetExtension(files.FileName);
                var fileName = "filenotifi-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                string path = Path.Combine(Server.MapPath("~/FileNotification"), fileName);
                model.FileName = Url.Content(Path.Combine("~/FileNotification/", fileName));
                NotificationModel notifi = new NotificationModel();

                if (notifi.AddNoTification(model, Session["UserName"].ToString()))
                {
                    files.SaveAs(path);
                    ViewBag.Message = "Employee details added successfully";
                }
                else
                {
                    ModelState.AddModelError("", "Error In Add File. Please Try Again !!!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Choose Correct File Type !!");

            }


         
            return RedirectToAction("Index", "Notification");


        }

        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public ActionResult DetailNotification(string IdNo)
        {

            var model = notify.detailNotification(IdNo);
            return View(model);

        }
        // hien thi thong bao va chi tiet thong bao
   
        public ActionResult DetailNo(long id)
        {
            // return connect.Notifications.Where(x => x.IdNo == id).ToList();
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.Where(x => x.IdNo == id).ToList();
            ViewBag.Detail = qLNCKHDHTDTD.Notifications.Where(x => x.IdNo == id).ToList(); 
            return View();
        }
      
    }
}