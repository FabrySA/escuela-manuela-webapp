using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebEscuelaProject.Models.Filters
{
    public class AlreadyLoggedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("username") != null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Home"},
                        {"action", "Index"}
                    }
                );
            }
            base.OnActionExecuting(context);
        }
    }

    public class NotLoggedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("username") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Account"},
                        {"action", "Login"}
                    }
                );
            }
            base.OnActionExecuting(context);
        }
    }

    public class NotAuthorizedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("username") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Account"},
                        {"action", "Login"}
                    }
                );
            }
            else if (context.HttpContext.Session.GetString("user_role") != "admin")
            {
                context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                       {"controller", "Home"},
                       {"action", "Index"}
                   }
               );
            }

            base.OnActionExecuting(context);
        }
    }
}
