namespace Blog.Web.Filters
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ForbidAuthorizedAttribute : Attribute, IActionFilter, IPageFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home");
            }
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home");
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home");
            }
        }
              
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
}
