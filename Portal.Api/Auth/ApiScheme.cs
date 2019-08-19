using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Portal.Api.Auth
{
    public class ApiKeyAuth : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public ApiKeyAuth(
             IOptionsMonitor<AuthenticationSchemeOptions> options,
             ILoggerFactory logger,
             UrlEncoder encoder,
             ISystemClock clock)
             : base(options, logger, encoder, clock)
        {
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey("ApiKey"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing ApiToken"));
            }
            try
            {
                var identity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Sid, "1") }, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception)
            {

                return Task.FromResult(AuthenticateResult.Fail("Invalid Authentication Failed"));
            }

        }
    }
}
