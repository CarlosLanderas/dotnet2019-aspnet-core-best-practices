using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2019.Api.Infrastructure.Middleware
{
    public class SecretMiddleware : IMiddleware
    {
        private readonly DiagnosticListener _diagnosticListener;
        private ReadOnlyMemory<byte> fileContent;
        private ReadOnlyMemory<byte> startParagraph;
        private ReadOnlyMemory<byte> endParagraph;

        public SecretMiddleware(DiagnosticListener diagnosticListener)
        {
            _diagnosticListener = diagnosticListener;
            fileContent = File.ReadAllBytes("DataContent.txt");
            startParagraph = Encoding.UTF8.GetBytes("<p>");
            endParagraph = Encoding.UTF8.GetBytes("</p>");
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var bytesConsumed = 0;
            var linePosition = 0;
            var writer = context.Response.BodyWriter;
            context.Response.ContentType = "text/html";

            do
            {
                linePosition = fileContent.Slice(bytesConsumed).Span.IndexOf((byte)'\n');

                if (linePosition >= 0)
                {                    
                    var lineLength = (bytesConsumed + linePosition) - bytesConsumed;

                    if(lineLength > 1)
                    {                        
                        await writer.WriteAsync(startParagraph);
                        await writer.WriteAsync(fileContent.Slice(bytesConsumed, lineLength));
                        await writer.WriteAsync(endParagraph);
                    }

                    bytesConsumed += lineLength + 1;
                }
            }
            while (linePosition >= 0);
        }
    }
}
