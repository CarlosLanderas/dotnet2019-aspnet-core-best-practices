using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using DotNet2019.Api;
using DotNet2019.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionalTests.Seedwork
{
    class TestStartup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public TestStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.ConfigureServices(services, environment)
                .AddDbContext<DataContext>(setup =>
                {
                    setup.UseSqlServer(configuration.GetConnectionString("SqlServer"), sql =>
                    {
                        sql.MigrationsAssembly(typeof(DataContext).Assembly.GetName().Name);
                    });
                })
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
