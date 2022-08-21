using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
   public class BannerMetaData
    {
        public long Id { get; set; }
        [Display(Name ="Image Url")]
        public string ImgUrl { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        public string Heading { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        [Display(Name ="Animation Type")]
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Select")]
        public int AnimationType { get; set; }
    }
    [MetadataType(typeof(BannerMetaData))]
    public partial class Banner
    {

    }
}
