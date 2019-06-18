using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DotNet2019.Host.Infrastructure.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyHandlerOptions>
    {
        public const string API_KEY_HEADER_NAME = "X-API-KEY";
        private readonly DiagnosticListener _listener;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, DiagnosticListener listener)
            : base(options, logger, encoder, clock) {
            _listener = listener;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Context.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();
                    

            if (apiKey == null) return Task.FromResult(AuthenticateResult.NoResult());

            if (Options.ApiKeys.Contains(apiKey))
            {
                var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity
                    (new[] {
                        new Claim(ClaimTypes.Name, "ApiUser"),
                        new Claim(ClaimTypes.Role, "ApiUserRole")
                    })
                });
                var ticket = new AuthenticationTicket(principal, "Test");

                if(_listener.IsEnabled())
                {
                    _listener.Write("Api.Diagnostics.ApiKey.Authentication.Success", new
                    {
                        HttpContext = Context,
                        ApiKey = apiKey
                    });
                }              
                
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid key"));
        }
    }

    public class ApiKeyHandlerOptions : AuthenticationSchemeOptions
    {
        public List<string> ApiKeys = new List<string>();
    }
}
