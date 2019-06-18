using DotNet2019.Api;
using DotNet2019.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet2019.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Api.Configuration.ConfigureServices(services, Environment)
                .AddEntityFrameworkCore(Configuration)
                .AddCustomAuthentication()
                .AddCustomAuthorization()
                .AddCustomHealthChecks()
                .AddHostingDiagnosticHandler();
        }

        public void Configure(IApplicationBuilder app)
        {           
            Api.Configuration.Configure(app, host =>
            {
                return host
                    .UseDefaultFiles()
                    .UseStaticFiles()
                    .UseCustomHealthchecks()
                    .UseHeaderDiagnostics();                   
            });
        }
    }
}
