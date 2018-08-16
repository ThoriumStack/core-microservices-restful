﻿using Microsoft.AspNetCore.Builder;
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
            base.ConfigureCustomServices(services);
        }
        
        public override void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            base.ConfigureApp(app, env);
        }
    }
}