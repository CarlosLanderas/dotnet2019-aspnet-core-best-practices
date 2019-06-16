using DotNet2019.Api.Infrastructure.Middleware;
using DotNet2019.Api.Infrastructure.Polly;
using DotNet2019.Api.Services;
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
                .AddScoped<SecretMiddleware>()
                .AddSingleton<ErrorMiddleware>()
                .AddCustomProblemDetails(environment)
                .AddCustomApiBehaviour()
                .AddHttpClient<ISomeService, SomeService>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler((serviceProvider, request) => RetryPolicy.GetPolicyWithJitterStrategy(serviceProvider))
                 .Services;
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
