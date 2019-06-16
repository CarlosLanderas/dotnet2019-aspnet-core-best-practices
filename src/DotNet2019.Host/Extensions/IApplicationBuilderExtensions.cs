using Microsoft.AspNetCore.Builder;
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
            return app.Use((context, next) =>
            {                
                var listener = app.ApplicationServices.GetService<DiagnosticListener>();
                var headers = string.Join("|",context.Request.Headers.Values.Select(h => h.ToString()));
                listener.StartActivity(new Activity("Api.Header.Diagnostics"), headers);
                return next();
            });
        }
    }
}
