using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            return services.AddHealthChecks()
                .AddSqlServer("server=localhost;initial catalog=master;user id=sa;password=Password12!", tags:  new[] { "dependencies" })
                .AddRedis("server=localhost", tags: new[] { "dependencies" })
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .Services;               
        }

    }
}
