using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Enums
{
    public enum CategoryType
    {
        [Display(Name = "Navigation Heading")]
        NavigationHeading = 1,
        [Display(Name = "Navigation Category")]
        NavigationCategory = 2,
        [Display(Name = "Navigation Link")]
        NavigationLink = 3
    }
}
