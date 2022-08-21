using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Enums
{
    public enum OrderStatus
    {
        New = 0,
        Inprocess = 1,
        Rejected = 2,
        Shipped = 3,
        ReturnRequest = 4,
        Returned = 5,
        Cancel=6,
    }
}
