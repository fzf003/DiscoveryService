using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Ocelot.Provider.Consul;
using DiscoveryService;
    using Swashbuckle.AspNetCore.Swagger;
    using System.IO;


namespace ConsulGatewayApp
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
            
            //services.AddOcelot();

            services.AddOcelot().AddConsul();


            services.AddSwaggerGen(c =>
                   {
                       c.SwaggerDoc("v1", new Info { Title = "GateWayService", Version = "v1" });
                      
                   });
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
         }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc()
             .UseSwagger().UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/a/swagger.json", "ApiSerice");
                        c.SwaggerEndpoint("/a/swagger.json", "TestSerice");
                        c.SwaggerEndpoint ("/swagger/v1/swagger.json", "gatewayservice");
                    });

                     app.UseDiscoveryService();
             
              app.UseOcelot().Wait();
        }
    }
}