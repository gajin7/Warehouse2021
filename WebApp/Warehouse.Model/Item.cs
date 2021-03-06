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
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.ReceiptItems = new HashSet<ReceiptItem>();
            this.ItemReports = new HashSet<ItemReport>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Type { get; set; }
        public string ShelfId { get; set; }
        public string PricelistId { get; set; }
    
        public virtual Shelf Shelf { get; set; }
        public virtual ItemType ItemType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiptItem> ReceiptItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemReport> ItemReports { get; set; }
        public virtual Pricelist Pricelist { get; set; }
    }
}
