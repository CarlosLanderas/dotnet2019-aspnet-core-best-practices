using DotNet2019.Api.Infrastructure.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
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
    public class HostingDiagnosticHandler : IHostedService, IObserver<KeyValuePair<string, object>>, IObserver<DiagnosticListener>, IDisposable
    {
        private readonly DiagnosticListener _diagnosticListener;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<DiagnosticsHub> _diagnosticsHub;
        private IDisposable _subscription;
        private List<IDisposable> _listenersSubscriptions = new List<IDisposable>();
        private static Dictionary<string, StringBuilder> traces = new Dictionary<string, StringBuilder>();
        private string requestBegin = "---------- REQUEST {0} BEGIN ----------\n";
        private string requestEnd = "---------- REQUEST {0} END ----------\n";

        public HostingDiagnosticHandler(DiagnosticListener diagnosticListener, IServiceScopeFactory scopeFactory, IHubContext<DiagnosticsHub> diagnosticsHub)
        {
            _diagnosticListener = diagnosticListener;
            _scopeFactory = scopeFactory;
            _diagnosticsHub = diagnosticsHub;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscription = DiagnosticListener.AllListeners.Subscribe(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void OnNext(DiagnosticListener value)
        {
            _listenersSubscriptions.Add(value.Subscribe(this));
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

                        var trace = (traces[requestId.ToString()]
                            .Append($"[HttpStop] Request finished with code {context.Response.StatusCode} \n")
                            .Append(string.Format(requestEnd, requestId))
                            .ToString());

                        Task.Run(async () =>
                        {
                            await _diagnosticsHub.Clients.All.SendAsync("SendDiagnotics", trace);
                        });                        

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

                case "Microsoft.AspNetCore.Mvc.BeforeAction":
                    {
                        var context = GetProperty<HttpContext>(@event.Value, "httpContext");
                        var requestId = context.Items["RequestId"].ToString();
                        var actionDescriptor = GetProperty<object>(@event.Value, "actionDescriptor");
                        var actionName = GetProperty<string>(actionDescriptor, "DisplayName");
                        traces[requestId].Append($"[Mvc Action] Executing {actionName}\n");
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


                case "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting":
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext;
                            var requestId = context.Items["RequestId"].ToString();
                            var payload = (CommandEventData)@event.Value;
                            traces[requestId].Append($"[EF Command] - {payload.Command.CommandText}\n");
                        }
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
            var prop = value.GetType().GetProperty(property);
            if (prop != null)
            {
                return (T)prop.GetValue(value);
            }

            return default(T);
            
        }

        public void OnCompleted()
        {

        }
        public void OnError(Exception error)
        {

        }

        public void Dispose()
        {
            _subscription.Dispose();
            foreach (var listenerSubscription in _listenersSubscriptions)
            {
                listenerSubscription.Dispose();
            }
        }
    }
}
