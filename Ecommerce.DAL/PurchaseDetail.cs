//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecommerce.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseDetail
    {
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        public long ProductId { get; set; }
        public long StockId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Purchase Purchase { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual Product Product { get; set; }
    }
}
