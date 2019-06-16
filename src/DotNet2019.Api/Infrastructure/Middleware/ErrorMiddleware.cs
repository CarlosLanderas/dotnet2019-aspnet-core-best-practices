using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DotNet2019.Api.Infrastructure.Middleware
{
    public class ErrorMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/middleware", out _, out var remaining))
            {
                if (remaining.StartsWithSegments("/error"))
                {
                    throw new Exception("This is an exception thrown from middleware.");
                }
            }

            return next(context);
        }
    }
}
