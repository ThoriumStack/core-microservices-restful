using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBucks.Core.MicroServices;
using Serilog;

namespace MyBucks.Core.Restful.Filters
{
    public class CheckKongAuthHeader : IActionFilter
    {
        private ILogger _logger;

        public CheckKongAuthHeader()
        {
            _logger = ServiceStartup.GetContainer().GetInstance<ILogger>();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Filters.Any(c=> c is AllowAnonymousFilter))
            {
                return;
            }
            
            if (!context.HttpContext.Request.Headers.ContainsKey("x-consumer-id"))
            {
                return;
            }
            if (!context.HttpContext.Request.Headers.ContainsKey("x-authenticated-userid"))
            {
                _logger.Information("Authentication failed for call. Anonymous calls not allowed.");
                context.Result = new StatusCodeResult((int) HttpStatusCode.Forbidden);
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            _logger.Information("{method} : {uri} {authuser}", context.HttpContext.Request.Method, context.HttpContext.Request.Path, context.HttpContext.Request.Headers["x-authenticated-userid"]);
        }
    }
}