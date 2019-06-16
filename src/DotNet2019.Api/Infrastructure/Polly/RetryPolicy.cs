using DotNet2019.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;

namespace DotNet2019.Api.Infrastructure.Polly
{
    public static class RetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetPolicy(IServiceProvider serviceProvider)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(message => message.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(
                    retryCount: 6, 
                    sleepDurationProvider: retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)), 
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        serviceProvider.GetService<ILogger<SomeService>>()
                            .LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                    });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetPolicyWithJitterStrategy(IServiceProvider serviceProvider)
        {
            var jitterer = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(message => message.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(
                    retryCount: 6,
                    sleepDurationProvider: retryAttemp => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)) +
                        TimeSpan.FromMilliseconds(jitterer.Next(0, 100)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        serviceProvider.GetService<ILogger<SomeService>>()
                            .LogWarning("Delaying for {delay}ms, then making retry {retry} with jitter strategy.", timespan.TotalMilliseconds, retryAttempt);
                    });
        }
    }
}
