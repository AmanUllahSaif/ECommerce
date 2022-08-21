using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.DAL;
using System.ComponentModel.DataAnnotations;
using Ecommerce.DAL.Resources;

namespace ECommerce.UI.Models
{
    public class StockViewModel
    {
        public List<AttribCombinationDetail> CombinationDetail { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        public decimal Quantity { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Select")]
        [Display(Name ="Product")]
        public long ProductId { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources),ErrorMessageResourceName ="Required")]
        [Display(Name ="Purchase Price")]
        public decimal PurchasePrice { get; set; }
    }
}