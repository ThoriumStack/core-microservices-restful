using Microsoft.AspNetCore.Hosting;
using Serilog;
using Thorium.Core.MicroServices.Abstractions;
using Thorium.Core.MicroServices.ConfigurationModels;

namespace Thorium.Core.MicroServices.Restful
{
    public class DefaultWebServiceEndpoint<TStartup> : IServiceEndpoint where TStartup : class
    {
       
            private ILogger _logger;
            private WebServiceSettings _webServiceSettings = null;
            private IWebHost _host;

            public DefaultWebServiceEndpoint(ILogger logger, WebServiceSettings webServiceSettings)
            {
                _logger = logger;
                _webServiceSettings = webServiceSettings;
            }

            public void StartServer()
            {

                var url = $"http://*:{_webServiceSettings.PortNumber}";
                _host = new WebHostBuilder()
                    .UseUrls(url)
                    .UseKestrel()
                    .UseSerilog(_logger)
                    .UseStartup<TStartup>()
                    .Build();
                _host.Start();
                _logger.Information("Hosting on {host}", url);
            }

            public void StopServer()
            {
                _logger.Information("Stopping API host");
                _host.StopAsync().Wait();
            }

            public virtual string EndpointDescription => "Restful API Service";
        
    }
}