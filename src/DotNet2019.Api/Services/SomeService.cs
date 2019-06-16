using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNet2019.Api.Services
{
    public class SomeService : ISomeService
    {
        private readonly HttpClient httpClient;

        public SomeService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<string> Ping()
        {
            return httpClient.GetStringAsync("https://localhost:5001/api/error");
        }
    }
}
