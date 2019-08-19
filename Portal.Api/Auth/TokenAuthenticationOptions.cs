using Microsoft.AspNetCore.Authentication;

namespace Portal.Api.Auth
{
    public class TokenAuthenticationOptions:AuthenticationSchemeOptions
    {
        public string[] ValidAudiences { get; set; }
        public string ValidIssuer { get; set; }
        public TokenAuthenticationOptions()
        {

        }
    }
}