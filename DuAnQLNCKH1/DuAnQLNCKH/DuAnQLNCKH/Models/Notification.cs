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
    
    public partial class Notification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Notification()
        {
            this.DetailNotifis = new HashSet<DetailNotifi>();
        }
    
        public int IdNo { get; set; }
        public System.DateTime DateCreat { get; set; }
        public string PersonCreat { get; set; }
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public string Object { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailNotifi> DetailNotifis { get; set; }
    }
}
