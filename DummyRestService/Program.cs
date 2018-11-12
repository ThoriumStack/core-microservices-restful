using System;
using Microsoft.Extensions.Configuration;
using MyBucks.Core.MicroServices;
using MyBucks.Core.MicroServices.Abstractions;
using MyBucks.Core.MicroServices.ConfigurationModels;
using MyBucks.Core.MicroServices.LivenessChecks;
using MyBucks.Core.MicroServices.Restful;
using Serilog;

namespace DummyRestService
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceRunner = new ServiceRunner();

            serviceRunner.Run(new DummyWebStartup(), args);
        }
    }

    public class DummyWebStartup : IServiceStartup, ICanCheckLiveness
    {
        public void ConfigureService(ServiceConfiguration configuration)
        {
            configuration.AddConfiguration<WebServiceSettings>();
            configuration.AddServiceEndpoint<DefaultWebServiceEndpoint<MyRestService>>();
        }

        public void ConfigureLivenessChecks(LivenessCheckConfiguration config)
        {
            config.AddCheck<SuperHealthCheck>();
        }
    }

    public class SuperHealthCheck : ILivenessCheck
    {
        private ILogger _logger;

        public SuperHealthCheck(ILogger logger)
        {
            _logger = logger;
        }
        
        public bool IsLive()
        {
            var val = (DateTime.Now.Second % 2 == 0);
            _logger.Information($"Live: {val}: {DateTime.Now.Second}");
            return val;
        }
    }

    public class MyRestService : BasicRestService
    {
        public MyRestService() : base(ServiceStartup.GetConfigurationRoot(), new DummyWebStartup())
        {
        }
    }
}