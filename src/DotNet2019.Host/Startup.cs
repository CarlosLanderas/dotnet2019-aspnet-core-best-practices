using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet2019.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet2019.Host
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; set; }
        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.ConfigureServices(services, Environment);
            services
            .AddCustomAuthentication()
            .AddCustomAuthorization()
            .AddCustomHealthChecks()
            .AddHostingDiagnosticHandler();
        }

        public void Configure(IApplicationBuilder app)
        {
            Configuration.Configure(app, host =>
                host
                    .UseRouting()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("Hello DotNet 2019!");
                        });
                    })
            );
            
            Configuration.Configure(app, host =>
            {
                return host
                    .UseCustomHealthchecks()
                    .UseHeaderDiagnostics();                   
            });
        }
    }
}
