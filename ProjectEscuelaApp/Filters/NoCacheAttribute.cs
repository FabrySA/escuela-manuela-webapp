﻿using Microsoft.AspNetCore.Mvc.Filters;

public class NoCacheAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        context.HttpContext.Response.Headers.Add("Pragma", "no-cache");
        context.HttpContext.Response.Headers.Add("Expires", "0");

        base.OnResultExecuting(context);
    }
}
