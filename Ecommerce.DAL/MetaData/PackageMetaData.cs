using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
    public class PackageMetaData
    {
        public long Id { get; set; }
        [Display(Name = "Product")]
        public long ProductId { get; set; }
        [Display(Name = "Start Quantity")]
        public decimal StartQuantity { get; set; }
        [Display(Name = "End Quantity")]
        public decimal EndQuantity { get; set; }
        public decimal Price { get; set; }
    }
    [MetadataType(typeof(PackageMetaData))]
    public partial class Package
    {

    }
}
