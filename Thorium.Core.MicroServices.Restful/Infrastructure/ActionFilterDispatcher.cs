using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Thorium.Core.MicroServices.Restful.Infrastructure
{
    public sealed class ActionFilterDispatcher : IActionFilter
    {
        private readonly Func<Type, IEnumerable> container;

        public ActionFilterDispatcher(Func<Type, IEnumerable> container)
        {
            this.container = container;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IEnumerable<object> attributes =
                context.Controller.GetType().GetTypeInfo().GetCustomAttributes(true);

            var controllerActionDescriptor =
                context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                attributes = attributes
                    .Concat(controllerActionDescriptor.MethodInfo.GetCustomAttributes(true));
            }

            foreach (var attribute in attributes)
            {
                Type filterType = typeof(IActionFilter<>).MakeGenericType(attribute.GetType());
                IEnumerable filters = this.container.Invoke(filterType);

                foreach (dynamic actionFilter in filters)
                {
                    actionFilter.OnActionExecuting((dynamic)attribute, context);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}