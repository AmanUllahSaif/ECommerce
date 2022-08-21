using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ECommerce.UI.Util
{
    public class NotApprovedUserFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                bool check = filterContext.HttpContext.Request.Url.AbsolutePath.Equals("/Error/NotApproved") || filterContext.HttpContext.Request.Url.AbsolutePath.ToLower().Equals("/account/logoff");
                if (!user.Identity.IsApproved() && !check)
                {
                    filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Error" },
                    { "action", "NotApproved" }
                });
                }
            }
            //throw new NotImplementedException();
        }
    }
}