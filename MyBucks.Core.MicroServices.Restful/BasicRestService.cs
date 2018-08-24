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
        public BasicRestService(IConfiguration configuration, IServiceStartup startup) :
            base(configuration, startup)
        { }
        
        public override void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiVersioning(o => o.ApiVersionReader = new UrlSegmentApiVersionReader());
            
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "HTTP API",
                    Version = "v1"
                });
                
                // UseFullTypeNameInSchemaIds replacement for .NET Core
                options.CustomSchemaIds(x => x.FullName);
            });
            base.ConfigureCustomServices(services);
        }
        
        public override void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            
            base.ConfigureApp(app, env);
        }
    }
}