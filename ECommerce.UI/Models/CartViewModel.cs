using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.DAL;
using System.ComponentModel.DataAnnotations;
using Ecommerce.DAL.Resources;

namespace ECommerce.UI.Models
{
    public class CartViewModel
    {
        public Product Product { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources), ErrorMessageResourceName = "Required")]
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources), ErrorMessageResourceName = "Required")]
        public long StockId { get; set; }
        public AttributeCombination AttributeCombination { get; set; }
        public Order ShippingInfo { get; set; }
        [Required(ErrorMessageResourceType =typeof(ValidationResources), ErrorMessageResourceName = "Required")]
        public long PackageId { get; set; }
        public Package package { get; set; }
        public List<CustomShirtsOrder> CustomShirts { get; set; }
    }
}