using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL
{
    public class SaleBannerMetaData
    {
        public long Id { get; set; }
        [Display(Name ="Image Url")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        public string Text { get; set; }
        [Display(Name ="Page Url")]
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        public string PageUrl { get; set; }
        [Display(Name = "Text Color")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "Required")]
        public string TextColor { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(SaleBannerMetaData))]
    public partial class SaleBanner
    {

    }
}
