using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBucks.Core.MicroServices.Restful.Infrastructure
{
    /// <summary>
    /// please see https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=98
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public interface IActionFilter<TAttribute> where TAttribute : Attribute
    {
        void OnActionExecuting(TAttribute attribute, ActionExecutingContext context);
    }
}