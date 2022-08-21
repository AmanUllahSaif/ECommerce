using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
   public class CategoryMetaData
    {
        public long Id { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        public string Name { get; set; }
        [Display(Name ="Parent")]
        public Nullable<long> ParentId { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Select")]
        public int Type { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        [Display(Name="Adult Alert")]
        public bool IsAdultAlert { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }
}
