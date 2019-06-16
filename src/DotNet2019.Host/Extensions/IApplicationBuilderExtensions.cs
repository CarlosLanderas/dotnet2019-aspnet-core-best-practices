using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomHealthchecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = registration => registration.Name.Equals("self")
            });

            return app.UseHealthChecks("/ready", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("dependencies")
            });
        }

        public static IApplicationBuilder UseHeaderDiagnostics(this IApplicationBuilder app)
        {         
            var listener = app.ApplicationServices.GetService<DiagnosticListener>();

            if (listener.IsEnabled())
            {
                return app.Use((context, next) =>
                {
                    var headers = string.Join("|", context.Request.Headers.Values.Select(h => h.ToString()));
                    listener.Write("Api.Diagnostics.Headers", new { Headers = headers, HttpContext = context });
                    return next();
                });
            }

            return app;
          
        }
    }
}
