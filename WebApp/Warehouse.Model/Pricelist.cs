//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warehouse.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pricelist
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pricelist()
        {
            this.Items = new HashSet<Item>();
        }
    
        public string Id { get; set; }
        public string ItemId { get; set; }
        public double Value { get; set; }
        public System.DateTime ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
    }
}
