using DotNet2019.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Respawn;
using System.IO;

namespace FunctionalTests.Seedwork
{
    public class ServerFixture
    {
        static Checkpoint _checkpoint = new Checkpoint()
        {
            TablesToIgnore = new string[] { "__EFMigrationsHistory" },
            WithReseed = true
        };

        public TestServer Server { get; private set; }

        public ServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            var testServer = new TestServer();

            var host = Host.CreateDefaultBuilder()
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .ConfigureAppConfiguration(builder =>
                 {
                     builder
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddEnvironmentVariables();
                 })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder
                     .UseServer(testServer)
                     .UseStartup<TestStartup>();
                 }).Build();

            host.StartAsync().Wait();
            host.MigrateDatabase<DataContext>();

            Server = host.GetTestServer();
        }

        internal static void ResetDatabase()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration
                .GetConnectionString("SqlServer");

            var task = _checkpoint.Reset(connectionString);
            task.Wait();
        }
    }
}
