using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
   public class PurchaseMetaData
    {
        public long Id { get; set; }
        [Display(Name ="Vendor")]
        public long VendorId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreateBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(PurchaseMetaData))]
    public partial class Purchase
    {

    }
}
