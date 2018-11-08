using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DiscoveryService;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {  

              services.AddDiscoveryServiceClient(cfg =>
                 {
                     cfg.Address = new Uri("http://10.0.84.33:8500");
                 });

                 services.AddOcelot(Configuration as ConfigurationRoot);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
         }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
             
              app.UseOcelot().Wait();
        }
    }
}
