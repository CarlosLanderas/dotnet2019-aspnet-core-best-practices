using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

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
                 options.SuppressMapClientErrors = false;
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
