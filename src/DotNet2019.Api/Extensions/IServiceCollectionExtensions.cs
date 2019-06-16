using DotNet2019.Api.Infrastructure.Middleware;
using DotNet2019.Api.Infrastructure.Polly;
using DotNet2019.Api.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services) =>
            services
                .AddMvc()
                .AddApplicationPart(typeof(IServiceCollectionExtensions).Assembly)
                .Services;

        public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services) =>
            services
                .AddSingleton<SecretMiddleware>()
                .AddSingleton<ErrorMiddleware>();

        public static IServiceCollection AddCustomServices(this IServiceCollection services) =>
            services.
                AddHttpClient<ISomeService, SomeService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler((serviceProvider, request) => RetryPolicy.GetPolicyWithJitterStrategy(serviceProvider))
                .Services;

        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services, IWebHostEnvironment environment) =>
           services
               .AddProblemDetails(configure =>
               {
                   configure.IncludeExceptionDetails = _ => environment.EnvironmentName == "Development";
               });

        public static IServiceCollection AddCustomApiBehaviour(this IServiceCollection services)
        {          

            return services.Configure<ApiBehaviorOptions>(options =>
             {                 
                 options.SuppressModelStateInvalidFilter = false;                 
                 options.SuppressInferBindingSourcesForParameters = false;

                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var problemDetails = new ValidationProblemDetails(context.ModelState)
                     {
                         Instance = context.HttpContext.Request.Path,
                         Status = StatusCodes.Status400BadRequest,
                         Type = $"https://httpstatuses.com/400",
                         Detail = "Please refer to the errors property for additional details."
                     };
                     return new BadRequestObjectResult(problemDetails)
                     {
                         ContentTypes =
                         {
                                "application/problem+json",
                                "application/problem+xml"
                         }
                     };
                 };
             });
        }
    }
}
