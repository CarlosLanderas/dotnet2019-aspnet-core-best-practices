using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2019.Api
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddMvc()
                .AddApplicationPart(typeof(Configuration).Assembly)
                .Services;
        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            IWebHostEnvironment environment,
            Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
                .UseProblemDetails(configure =>
                {
                    configure.IncludeExceptionDetails = _ => environment.IsDevelopment();
                })
                .Use(CustomMiddleware)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }

        private static Task CustomMiddleware(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path.StartsWithSegments("/middleware", out _, out var remaining))
            {
                if (remaining.StartsWithSegments("/error"))
                {
                    throw new Exception("This is an exception thrown from middleware.");
                }
            }

            return next();
        }
    }
}
