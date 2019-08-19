using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Portal.Api.Auth
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        public IServiceProvider ServiceProvider { get; set; }

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider)
            : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            var token = headers.FirstOrDefault(x => x.Key == OAuthSettings.AuthHeaderName);

            if (string.IsNullOrEmpty(token.Value))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token is null"));
            }
            
            var validationParameters = new TokenValidationParameters();
            validationParameters.ValidAudiences = this.Options.ValidAudiences;
            validationParameters.ValidIssuer = this.Options.ValidIssuer;
            
            var isValidToken=ValidateToken(token.Value, validationParameters);

            if (!isValidToken)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Could not validate the token : for token={token.Value}"));
            }

            try
            {   
                var claims = new[] { new Claim("token", token.Value) };
                var identity = new ClaimsIdentity(claims, nameof(TokenAuthenticationHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity),new AuthenticationProperties(), this.Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authentication Failed"));
            }
        }

        private bool ValidateToken(string rawToken, TokenValidationParameters parameters)
        {
            var jwt = rawToken.Replace("Bearer ", string.Empty);
            var token = new JwtSecurityToken(jwt);
            var handler = new JwtSecurityTokenHandler();
            if (token.Issuer==parameters.ValidIssuer && token.Audiences.All (c=> parameters.ValidAudiences.Contains(c)))
            {
                return true;
            }

            /* proper validation or call introspect endpoint
            SecurityToken validatedToken;
            var claims=handler.ValidateToken(jwt, parameters, out validatedToken);
             */
             return false;
        }
    }
}
