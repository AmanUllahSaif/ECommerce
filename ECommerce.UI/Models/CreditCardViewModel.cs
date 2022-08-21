using Ecommerce.DAL.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.UI.Models
{
    public class CreditCardViewModel
    {
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string CardName { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string Number { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string CVV { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string ExpireMonth { get; set; }
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string ExpireYear { get; set; }
        [Display(Name ="Frist Name")]
        [Required(ErrorMessageResourceName ="Required", ErrorMessageResourceType =typeof(ValidationResources))]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationResources))]
        public string LastName { get; set; }
    }
}