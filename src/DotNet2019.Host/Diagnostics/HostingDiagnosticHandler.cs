using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet2019.Host.Diagnostics
{
    public class HostingDiagnosticHandler : IHostedService, IObserver<KeyValuePair<string, object>>, IDisposable
    {
        private readonly DiagnosticListener _diagnosticListener;
        private IDisposable _subscription;

        public HostingDiagnosticHandler(DiagnosticListener diagnosticListener)
        {
            _diagnosticListener = diagnosticListener;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscription = _diagnosticListener.Subscribe(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public void OnNext(KeyValuePair<string, object> @event)
        {
            switch (@event.Key)
            {

                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start":
                    {
                        var context = GetContext(@event.Value);
                        var requestId = Guid.NewGuid();
                        context.Items.Add("RequestId", requestId);
                        Console.WriteLine($"[Diagnostics] Incoming request {requestId} {context.Request.Path} VERB: {context.Request.Method}");

                    }
                    break;
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop":
                    {
                        var context = GetContext(@event.Value);
                        var requestId = context.Items["RequestId"];
                        Console.WriteLine($"[Diagnostics] Incoming request {requestId.ToString()} " +
                            $"finished with code {context.Response.StatusCode} ");
                    }
                    break;

                case "Microsoft.AspNetCore.Diagnostics.UnhandledException":
                    {
                        var context = GetContext(@event.Value);
                        var ex = @event.Value as Exception;
                        var requestId = context.Items["RequestId"];
                        Console.WriteLine($"[Diagnostics] Request {requestId} failed with error {ex.Message}");
                    }
                    break;
                case "Api.Header.Diagnostics.Start":
                    {
                        Console.WriteLine($"[Diagnostics - RequestHeaders] {@event.Value.ToString()}");
                    }
                    break;
            }
        }

        private HttpContext GetContext(object value)
        {
            return (HttpContext)value.GetType().GetProperty("HttpContext").GetValue(value);
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }


    }
}
