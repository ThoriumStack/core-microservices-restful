using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MyBucks.Core.MicroServices.Restful.Infrastructure
{
    public class ProfilingActionFilterDecorator<TAttribute>
        : IActionFilter<TAttribute>
        where TAttribute : Attribute
    {
        private readonly IActionFilter<TAttribute> decoratee;
        private readonly ILogger logger;

        public ProfilingActionFilterDecorator(
            IActionFilter<TAttribute> decoratee, ILogger logger)
        {
            this.decoratee = decoratee;
            this.logger = logger;
        }

        public void OnActionExecuting(TAttribute attribute,
            ActionExecutingContext context)
        {
            Debug.WriteLine("Decorated OnActionExecuting.");
            this.decoratee.OnActionExecuting(attribute, context);
        }
    }
}