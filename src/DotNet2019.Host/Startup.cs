using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet2019.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNet2019.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Configuration.Configure(app, env, host =>
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

            
        }
    }
}
