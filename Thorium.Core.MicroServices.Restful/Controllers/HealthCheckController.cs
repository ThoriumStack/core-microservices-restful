using Microsoft.AspNetCore.Mvc;
using Thorium.Core.MicroServices.Abstractions;

namespace Thorium.Core.MicroServices.Restful.Controllers
{
    [ApiController]
    [Route("/health")]
    public class HealthCheckController : Controller
    {
        private readonly ILiveChecker _checker;

        public HealthCheckController(ILiveChecker checker)
        {
            _checker = checker;
        }

        // GET
        public IActionResult Index()
        {
            var result = _checker.RunChecks();

            return StatusCode(result ? 200 : 500);
        }
    }
}