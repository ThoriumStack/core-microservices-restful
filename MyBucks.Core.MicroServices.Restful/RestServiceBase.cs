using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBucks.Core.MicroServices.Abstractions;

namespace MyBucks.Core.MicroServices.Restful
{
   public abstract class BasicRestService : RestServiceBase
    {
        protected BasicRestService(IConfiguration configuration, IServiceStartup startup) :
            base(configuration, startup)
        { }
        
        protected new virtual void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiVersioning(o => o.ApiVersionReader = new UrlSegmentApiVersionReader());
        }
        
        protected new virtual void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            base.ConfigureApp(app, env);
        }
    }
}