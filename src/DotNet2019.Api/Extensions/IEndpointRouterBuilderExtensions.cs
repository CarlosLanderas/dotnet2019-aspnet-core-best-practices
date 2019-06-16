using DotNet2019.Api.Infrastructure.Middleware;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    public static class IEndpointRouterBuilderExtensions
    {
        public static IEndpointConventionBuilder MapSecretEndpoint(this IEndpointRouteBuilder endpoints)
        {
            var pipeline = endpoints.CreateApplicationBuilder()           
                .UseMiddleware<SecretMiddleware>()                
                .Build();

            return endpoints.Map("/secret", pipeline);
        }
    }
}
