using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Enums
{
    public enum ShirtType
    {
        [Display(Name = "Short Sleeve Round Neck")]
        ShortSleeveRound = 1,
        [Display(Name = "Short Sleeve V Neck")]
        ShortSleeveV =2,
        //[Display(Name = "Long Sleeve Round Neck")]
        //LongSleeveRound = 3,
        //[Display(Name = "Long Sleeve V Neck")]
        //LongSleeveV = 4
    }
}
