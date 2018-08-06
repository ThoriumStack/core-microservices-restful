using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBucks.Core.MicroServices.Abstractions;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace MyBucks.Core.MicroServices.Restful
{
    public abstract class RestServiceBase
    {
        private readonly IServiceStartup _startup;
        private readonly IConfiguration _configuration;
        private readonly Container _container = new Container();
        
        public RestServiceBase(IConfiguration configuration, IServiceStartup startup)
        {
            _configuration = configuration;
            _startup = startup;
        }

        public IConfiguration Configuration => _configuration;
        
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCustomServices(services);
            IntegrateSimpleInjector(services);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);
            ConfigureApp(app, env);
        }
        
        public virtual void ConfigureCustomServices(IServiceCollection services)
        {
        }
        
        public virtual void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
        
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }
        
        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            LoadAppServices();

            // Cross-wire ASP.NET services (if any). For instance:
            _container.CrossWire<ILoggerFactory>(app);
            _container.Verify();
            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
        
        private void LoadAppServices()
        {
            ServiceStartup.ContainerSetup(_container, _startup);
        }
    }
}