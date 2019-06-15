using DotNet2019.Api.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNet2019.Api
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            return services                
                .AddMvc()
                .Services
                .AddScoped<SecretMiddleware>()
                .AddCustomProblemDetails(environment);
        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
            .UseRouting()            
            .UseEndpoints(endpoints =>
              {                  
                  endpoints.MapControllerRoute(
                         name: "default",
                         pattern: "{controller=Home}/{action=Index}/{id?}");
                  endpoints.MapRazorPages();

                  endpoints.MapSecretEndpoint()

              });

        }
    }
}
