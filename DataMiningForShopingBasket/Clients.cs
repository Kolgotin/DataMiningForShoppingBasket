//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataMiningForShopingBasket
{
    using System;
    using System.Collections.Generic;
    
    public partial class Clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clients()
        {
            this.Sales = new HashSet<Sales>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public Nullable<int> GenderId { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    
        public virtual Genders Genders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sales> Sales { get; set; }
    }
}
