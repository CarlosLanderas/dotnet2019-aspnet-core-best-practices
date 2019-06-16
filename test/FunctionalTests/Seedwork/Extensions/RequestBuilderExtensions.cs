using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.TestHost
{
    [ExcludeFromCodeCoverage]
    public static class RequestBuilderExtensions
    {
        public static Task<HttpResponseMessage> PutAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Put);
        }
        public static Task<HttpResponseMessage> DeleteAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Delete);
        }

        public static RequestBuilder WithJsonBody<TContent>(this RequestBuilder builder, TContent content, string contentType = "application/json")
        {
            var json = JsonConvert.SerializeObject(content);

            return builder.And(message =>
            {
                message.Content = new StringContent(json, Encoding.UTF8, contentType);
            });
        }
    }
}
