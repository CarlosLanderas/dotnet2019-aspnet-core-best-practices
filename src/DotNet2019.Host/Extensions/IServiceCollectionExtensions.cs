using DotNet2019.Host.Diagnostics;
using DotNet2019.Host.Infrastructure.Authentication;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            return services.AddHealthChecks()
                .AddSqlServer("server=localhost;initial catalog=master;user id=sa;password=Password12!", tags: new[] { "dependencies" })
                .AddRedis("server=localhost", tags: new[] { "dependencies" })
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .Services;
        }

        public static IServiceCollection AddHostingDiagnosticHandler(this IServiceCollection services)
        {
            return services.AddHostedService<HostingDiagnosticHandler>();
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication("Test")
                .AddScheme<ApiKeyHandlerOptions, ApiKeyAuthenticationHandler>(
                authenticationScheme: "Test",
                configureOptions: setup =>
                 {
                     setup.ApiKeys.Add("PatataKey");
                 })
           .Services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization(config =>
            {
                config.AddPolicy("ApiKeyPolicy", config =>
                {                    
                    config.RequireRole("ApiUserRole");
                });
            });
        }

    }
}
