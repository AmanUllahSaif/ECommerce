using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ecommerce.DAL.Resources;

namespace Ecommerce.DAL
{
   public class ProductMetaData
    {
        public long Id { get; set; }
        [Display(Name ="Category")]
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public long CategoryId { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Name { get; set; }
        public string BarCode { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Description { get; set; }
        public bool Featured { get; set; }
        public bool Sale { get; set; }
        public decimal Price { get; set; }
        [Display(Name="Purchase Price")]
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public Nullable<decimal> PurchasePrice { get; set; }
        [Display(Name = "Weight (£)")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationResources))]
        public Nullable<decimal> Weight { get; set; }
        [Display(Name = "Height (Cm)")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationResources))]
        public Nullable<decimal> Height { get; set; }
        [Display(Name = "Width (Cm)")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationResources))]
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        [Display(Name="Profit Margin")]
        public decimal ProfitMargin { get; set; }
    }
}
