using DotNet2019.Api.Infrastructure.Middleware;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DotNet2019.Api
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            return services
                .AddMvc()
                .AddApplicationPart(typeof(Configuration).Assembly)
                .Services
                .AddScoped<SecretMiddleware>()
                .AddCustomProblemDetails(environment);
        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
                .UseProblemDetails()
                .Use(CustomMiddleware)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                })
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                  {
                      endpoints.MapControllerRoute(
                             name: "default",
                             pattern: "{controller=Home}/{action=Index}/{id?}");
                      endpoints.MapRazorPages();

                      endpoints.MapSecretEndpoint().RequireAuthorization("ApiKeyPolicy");

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
