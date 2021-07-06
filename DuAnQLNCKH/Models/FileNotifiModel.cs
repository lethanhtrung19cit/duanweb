using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class FileNotifiModel
    {
        public DateTime DateCreat { get; set; }
        public string PersonCreat { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public List<FileNotifiModel> ListFile { get; set; }
    }
}