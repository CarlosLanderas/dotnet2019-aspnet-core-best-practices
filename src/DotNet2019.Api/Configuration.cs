using DotNet2019.Api.Infrastructure.Middleware;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                .AddApplicationPart(typeof(Configuration).Assembly)
                .Services
                .AddSingleton<SecretMiddleware>()
                .AddSingleton<ErrorMiddleware>()
                .AddCustomProblemDetails(environment)
                .AddCustomApiBehaviour();
        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
                .UseProblemDetails()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ErrorMiddleware>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();

                    endpoints.MapSecretEndpoint().RequireAuthorization("ApiKeyPolicy");
                });
        }
    }
}
