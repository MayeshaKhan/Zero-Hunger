//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroHunger.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            this.Foods = new HashSet<Food>();
        }
    
        public int Id { get; set; }
        public string Status { get; set; }
        public System.DateTime Expiry { get; set; }
        public int Quantity { get; set; }
        public System.DateTime OrderTime { get; set; }
        public int Res_Id { get; set; }
        public Nullable<int> Admin_Id { get; set; }
        public Nullable<int> Emp_Id { get; set; }
    
        public virtual Admin Admin { get; set; }
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Food> Foods { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
