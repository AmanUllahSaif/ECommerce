using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
    public class PurchaseDetailMetaData
    {
        public long Id { get; set; }
        [Display(Name ="Purchase")]
        public long PurchaseId { get; set; }
        [Display(Name ="Product")]
        public long ProductId { get; set; }
        [Display(Name ="Stock")]
        public long StockId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(PurchaseDetailMetaData))]
    public partial class PurchaseDetail
    {

    }
}
