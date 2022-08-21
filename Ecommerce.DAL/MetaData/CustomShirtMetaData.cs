using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
    class CustomShirtMetaData
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationResources))]
        public Nullable<int> Size { get; set; }
    }

    public partial class CustomShirt
    {
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
    }
}
