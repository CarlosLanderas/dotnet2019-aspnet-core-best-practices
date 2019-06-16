using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace FunctionalTests.Seedwork
{
    public class ServerFixture
    {
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
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder
                     .UseServer(testServer)
                     .UseStartup<TestStartup>();
                 }).Build();

            host.StartAsync().Wait();

            Server = host.GetTestServer();
        }
    }
}
