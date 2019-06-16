using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet2019.Api.Infrastructure.Middleware
{
    public class SecretMiddleware : IMiddleware
    {
        private readonly DiagnosticListener _diagnosticListener;
        private ReadOnlyMemory<byte> fileContent;
        
        public SecretMiddleware(DiagnosticListener diagnosticListener)
        {
            _diagnosticListener = diagnosticListener;
            fileContent = File.ReadAllBytes("DataContent.txt");
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {  
   
            var bytesConsumed = 0;
            var linePosition = 0;

            do
            {
                linePosition = fileContent.Slice(bytesConsumed).Span.IndexOf((byte)'\n');

                if (linePosition >= 0)
                {                    
                    var lineLength = (bytesConsumed + linePosition) - bytesConsumed;

                    await context.Response.BodyWriter.WriteAsync(fileContent.Slice(bytesConsumed, lineLength));

                    bytesConsumed += lineLength + 1;
                }
            }
            while (linePosition >= 0);
        }
    }
}
