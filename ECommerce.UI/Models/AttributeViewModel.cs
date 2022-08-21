using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.DAL;

namespace ECommerce.UI.Models
{
    public class AttributeViewModel
    {
        public Ecommerce.DAL.Attribute Attribute { get; set; }
        public List<AttributeValue> Values { get; set; }

    }
}