using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DAL.Enums
{
    public enum CardType
    {
        Visa,
        Master,
        [Display(Name = "American Express")]
        AmericanExpress,
    }
}
