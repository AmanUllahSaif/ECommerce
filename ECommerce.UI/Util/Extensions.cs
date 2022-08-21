using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ECommerce.UI.Util
{
    public static class Extensions
    {
        public static long GetCustomId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("userId");
            // Test for null to avoid issues during local testing
            return (claim != null) ? Convert.ToInt64(claim.Value) : 0;
        }
        /// <summary>
        /// Checks that Current user is approved by admin or not
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static bool IsApproved(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Approved");
            // Test for null to avoid issues during local testing
            return Convert.ToBoolean(claim.Value);
            //return (claim != null) ? Convert.ToBoolean(claim.Value) : false;
        }
    }
}