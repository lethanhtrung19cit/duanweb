//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuAnQLNCKH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Extension
    {
        public int IdEx { get; set; }
        public string IdTp { get; set; }
        public System.DateTime Times { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    
        public virtual TopicOfLecture TopicOfLecture { get; set; }
    }
}