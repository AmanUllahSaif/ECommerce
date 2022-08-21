using Ecommerce.DAL;
using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.UI.Models
{
    public class ProductDetailViewModel
    {
        public Product Item { get; set; }
        public List<AttribCombinationDetail> Veriations { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Should be more then 0")]
        public decimal Quantity { get; set; }
        public long PacakgeId { get; set; }
    }
}