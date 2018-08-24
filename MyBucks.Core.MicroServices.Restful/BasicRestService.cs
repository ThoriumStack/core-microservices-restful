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
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseStaticFiles();
            
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API V1");
                });
            
            app.UseMvc();
            
            base.ConfigureApp(app, env);
        }
    }
}