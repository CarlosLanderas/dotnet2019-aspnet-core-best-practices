using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet2019.Host.Diagnostics
{
    public class HostingDiagnosticHandler : IHostedService, IObserver<KeyValuePair<string, object>>, IDisposable
    {
        private readonly DiagnosticListener _diagnosticListener;
        private IDisposable _subscription;
        private static Dictionary<string, StringBuilder> traces = new Dictionary<string, StringBuilder>();
        private string requestBegin = "---------- REQUEST {0} BEGIN ----------\n";
        private string requestEnd = "---------- REQUEST {0} END ----------\n";

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

                        var builder = new StringBuilder(string.Format(requestBegin, requestId))
                            .Append($"[HttpStart] Incoming request {context.Request.Path} VERB: {context.Request.Method}\n");
                        traces[requestId.ToString()] = builder;
                    }
                    break;
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop":
                    {
                        var context = GetContext(@event.Value);
                        var requestId = context.Items["RequestId"].ToString();

                        Console.WriteLine(traces[requestId.ToString()]
                            .Append($"[HttpStop] Request finished with code {context.Response.StatusCode} \n")
                            .Append(string.Format(requestEnd, requestId)
                            .ToString()));

                        traces.Remove(requestId);
                    }
                    break;

                case "Microsoft.AspNetCore.Diagnostics.UnhandledException":
                    {
                        var context = GetContext(@event.Value);
                        var ex = @event.Value as Exception;
                        var requestId = context.Items["RequestId"].ToString();
                        traces[requestId].Append($"[Exception] Request {requestId} failed with error {ex.Message}\n");
                    }
                    break;
                case "Microsoft.AspNetCore.Mvc.AfterActionResult":
                    {
                        var actionContext = GetProperty<object>(@event.Value, "actionContext");
                        var context = GetProperty<HttpContext>(actionContext, "HttpContext");
                        object actionResult = GetProperty<object>(@event.Value, "result");
                        dynamic response = GetProperty<dynamic>(actionResult, "Value");
                        var requestId = context.Items["RequestId"].ToString();
                        traces[requestId].Append($"[ActionResultResponse] {JsonConvert.SerializeObject(response)}\n ");
                    }
                    break;

                case "Api.Diagnostics.Headers":
                    {
                        var context = GetContext(@event.Value);
                        var requestId = context.Items["RequestId"].ToString();

                        var headers = GetProperty<string>(@event.Value, "Headers");
                        traces[requestId].Append($"[Headers] {headers}\n");                        
                    }
                    break;

                case "Api.Diagnostics.ApiKey.Authentication.Success":
                   {
                        var context = GetContext(@event.Value);
                        var requestId = context.Items["RequestId"].ToString();
                        var apiKey = GetProperty<string>(@event.Value, "ApiKey");
                        traces[requestId].Append($"[Api Key Authentication] User logged with api key: {apiKey}\n");
                    }
                    break;
            }
        }

        private HttpContext GetContext(object value)
        {
            return (HttpContext)value.GetType().GetProperty("HttpContext").GetValue(value);
        }

        private T GetProperty<T>(object value, string property)
        {
            return (T)value.GetType().GetProperty(property).GetValue(value);
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }


    }
}
