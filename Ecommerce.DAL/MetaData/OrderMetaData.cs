using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
   public class OrderMetaData
    {
        public long Id { get; set; }
        [Display(Name ="User")]
        public Nullable<long> UserId { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessageResourceName ="Email", ErrorMessageResourceType =typeof(ValidationResources))]
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Country { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string City { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Address { get; set; }
        [Display(Name ="Postal Code")]
        [DataType(DataType.PostalCode, ErrorMessageResourceName ="PostalCode", ErrorMessageResourceType = typeof(ValidationResources))]
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string PostalCode { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public int Status { get; set; }
        public Nullable<int> Type { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(OrderMetaData))]
    public partial class Order
    {

    }
}
