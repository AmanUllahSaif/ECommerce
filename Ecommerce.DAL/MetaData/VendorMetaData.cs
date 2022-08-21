using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL.Resources;

namespace Ecommerce.DAL
{
   public class VendorMetaData
    {
        public long Id { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "Required")]
        public string Contact { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {

    }
}
