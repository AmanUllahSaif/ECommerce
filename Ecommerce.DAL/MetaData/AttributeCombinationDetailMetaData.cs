using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
    public class AttributeCombinationDetailMetaData
    {
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public long AttributeId { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public long ValueId { get; set; }
    }

    [MetadataType(typeof(AttributeCombinationDetailMetaData))]
    public partial class AttribCombinationDetail
    {

    }
}
