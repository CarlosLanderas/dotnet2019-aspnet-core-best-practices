using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2019.Api.Infrastructure.Middleware
{
    public class SecretMiddleware : IMiddleware
    {
        private readonly DiagnosticListener _diagnosticListener;

        public SecretMiddleware(DiagnosticListener diagnosticListener)
        {
            _diagnosticListener = diagnosticListener;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var content = Encoding.UTF8.GetBytes("Hello from the secret endpoint");
            await context.Response.BodyWriter.WriteAsync(content);
        }
    }
}
