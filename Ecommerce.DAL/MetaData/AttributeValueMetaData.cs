using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
  public class AttributeValueMetaData
    {
        public long Id { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Value { get; set; }
        [Display(Name = "Attribute")]
        public long AttributeId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(AttributeValueMetaData))]
    public partial class AttributeValue
    {

    }
}
