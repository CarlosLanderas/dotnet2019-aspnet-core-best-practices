using DotNet2019.Api.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

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
