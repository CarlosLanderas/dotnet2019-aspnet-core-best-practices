using DotNet2019.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;

namespace FunctionalTests.Seedwork
{
    class TestStartup
    {
        private readonly IWebHostEnvironment environment;

        public TestStartup(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.ConfigureServices(services, environment)
                .AddAuthorization()
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddTestServer();
        }

        public void Configure(IApplicationBuilder app)
        {
            Configuration.Configure(app, host => host);
        }
    }
}
