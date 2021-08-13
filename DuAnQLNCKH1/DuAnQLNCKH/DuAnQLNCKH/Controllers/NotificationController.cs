﻿using DuAnQLNCKH.Models;
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
        //[NotMapped]
        //public IEnumerable<Information> EmailCollection { get; set; }
        //[NotMapped]
        //public string[] SelectedIdArray { get; set; }
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        NotificationModel notify = new NotificationModel();
        private SqlConnection con;
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
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


            ViewBag.listNoti = list1;
            ViewBag.NotificationsLec = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a => a.Object == "Giảng viên").ToList();
            ViewBag.NotificationsStu = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a => a.Object == "Sinh viên").ToList(); 
           
            List<TopicOfLecture> listProgress = qLNCKHDHTDTD.TopicOfLectures.ToList();


            ViewBag.listProgress = new MultiSelectList(qLNCKHDHTDTD.TopicOfLectures.Select(x => x.Progress).Distinct().ToList(), "Progress");
            ViewBag.listEmail = qLNCKHDHTDTD.Information.ToList();

             

        }
        public ActionResult Index()
        {
            
            selectedEmail ema = new selectedEmail();

           
            ViewBag.NotificationsLec = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a=>a.Object=="Giảng viên").ToList();
              
            List<TopicOfLecture> listProgress = qLNCKHDHTDTD.TopicOfLectures.ToList();

            ViewBag.listProgress = new MultiSelectList(qLNCKHDHTDTD.TopicOfLectures.Select(x => x.Progress).Distinct().ToList(), "Progress");
            ema.EmailCollection = qLNCKHDHTDTD.Information.ToList();

            
            ViewBag.listEmail = ema.EmailCollection;

            return View();
        }
        public ActionResult NotifiStu()
        {
           
            selectedEmail ema = new selectedEmail();


            ViewBag.NotificationsStu = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a=>a.Object=="Sinh viên").ToList();
 
             ema.EmailCollectionStu = qLNCKHDHTDTD.TopicOfStudents.ToList();

            ViewBag.listEmail = ema.EmailCollectionStu;

            return View();
        }
        [HttpPost]
        public ActionResult CreateNotifi(HttpPostedFileBase files, Notification model, string Object)
        {

            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    var Extension = Path.GetExtension(files.FileName);
                    var fileName = "filenotifi-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                    string path = Path.Combine(Server.MapPath("~/File/FileNotification"), fileName);
                    model.FileName = Url.Content(Path.Combine("~/File/FileNotification/", fileName));
                    NotificationModel notifi = new NotificationModel();

                    if (notifi.AddNoTification(model, Session["UserName"].ToString(), Object))
                    {
                        files.SaveAs(path);
                        ViewBag.Message = "Employee details added successfully";
                        viewbag();
                        if (Object.Equals("Giảng viên"))
                            return View("Index");
                        else return View("NotifiStu");
                    }
                     
                }
                else
                {
                    qLNCKHDHTDTD.Database.ExecuteSqlCommand("set dateformat dmy Insert into Notification(DateCreat, PersonCreat, Title, Content, Object) values ('" + DateTime.Parse(@DateTime.Now.ToString("dd/MM/yyyy")) + "', '" + Session["UserName"].ToString() + "', N'" + model.Title + "', N'" + model.Content + "', N'" + Object + "')");
                    ViewBag.Message = "Employee details added successfully";
                    viewbag();
                    if (Object.Equals("Giảng viên"))
                        return View("Index");
                    else return View("NotifiStu");
                }
            }
            viewbag();
            if (Object.Equals("Giảng viên"))
                return View("Index");
            else return View("NotifiStu");


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
            ViewBag.NotificationsLec = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a=>a.Object=="Giảng viên").ToList();
            ViewBag.NotificationsStu = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a => a.Object == "Sinh viên").ToList();

            var data = qLNCKHDHTDTD.Emails.FirstOrDefault();

            string from1 = data.NameEmail;

             using (MailMessage mail = new MailMessage())
            {
                 mail.From = new MailAddress(from1);
                mail.Body = Body;
                mail.Subject = Subject;
                 
                qLNCKHDHTDTD.SaveChanges();
                List<Information> information1 = new List<Information>();
               foreach (var i in emp.SelectedIdArray)
                {
                    mail.To.Add(i);

                }
               
                
                
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from1, data.PassWord);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
               

                return View("Index");
            }
            
            
           
        }
 
      
        
        public ActionResult getTypeList(string Progress)
        {
            List<TopicOfLecture> topicOfLectures = qLNCKHDHTDTD.TopicOfLectures.ToList();
 
            List<Information> informations = qLNCKHDHTDTD.Information.ToList();
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            if (Progress.Equals(""))
            {
                var listJournal = (from t in topicOfLectures
                                   join i in informations on t.IdLe equals i.IdLe
                                   where t.Status=="đã duyệt"
                                   select new
                                   {
                                       i.IdLe,
                                       i.Email

                                   }).Distinct().ToList();
                return Json(listJournal, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listJournal = (from t in topicOfLectures
                                   join i in informations on t.IdLe equals i.IdLe
                                   where t.Progress == Progress
                                   select new
                                   {
                                       i.IdLe,
                                       i.Email

                                   }).Distinct().ToList();
                 return Json(listJournal, JsonRequestBehavior.AllowGet);
            }    

        }

        public ActionResult getTypeList1()
        {
            List<TopicOfLecture> topicOfLectures = qLNCKHDHTDTD.TopicOfLectures.ToList();

            List<Information> informations = qLNCKHDHTDTD.Information.ToList();
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            var listJournal = (from t in topicOfLectures
                               join i in informations on t.IdLe equals i.IdLe
                               where t.Status == "đã duyệt"
                               select new
                               {
                                   i.IdLe,
                                   i.Email

                               }).Distinct().ToList();

            return Json(listJournal, JsonRequestBehavior.AllowGet);
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