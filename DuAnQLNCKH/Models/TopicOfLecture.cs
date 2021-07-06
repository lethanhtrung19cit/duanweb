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
    
    public partial class TopicOfLecture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TopicOfLecture()
        {
            this.DetailStatementLes = new HashSet<DetailStatementLe>();
            this.Extensions = new HashSet<Extension>();
            this.ProgressLes = new HashSet<ProgressLe>();
        }
    
        public string IdTp { get; set; }
        public string Name { get; set; }
        public string IdLe { get; set; }
        public Nullable<int> IdP { get; set; }
        public System.DateTime DateSt { get; set; }
        public int Times { get; set; }
        public Nullable<double> Expense { get; set; }
        public string Status { get; set; }
        public string Progress { get; set; }
        public int CountAuthor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailStatementLe> DetailStatementLes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Extension> Extensions { get; set; }
        public virtual Information Information { get; set; }
        public virtual PointTable PointTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgressLe> ProgressLes { get; set; }
    }
}
