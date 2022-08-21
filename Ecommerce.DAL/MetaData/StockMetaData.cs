using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
   public class StockMetaData
    {
        public long Id { get; set; }
        [Display(Name ="Attribute Combination")]
        public long AttribCombiId { get; set; }
        [Display(Name ="Product")]
        public long ProductId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(StockMetaData))]
    public partial class Stock
    {

    }
}
