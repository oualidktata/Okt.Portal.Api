using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace Portal.Api.Auth
{
    public class SecurityRequirementDocumentFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
    {
       

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var flows = new OpenApiOAuthFlows();
            flows.ClientCredentials = new OpenApiOAuthFlow()
            {
                //AuthorizationUrl = new Uri(OAuthSettings.Auth, UriKind.Absolute),
                TokenUrl = new Uri(OAuthSettings.OktaTokenUrl, UriKind.Absolute),
                Scopes = OAuthSettings.Scopes
            };
            var oauthScheme = new OpenApiSecurityScheme()
            {

                Type = SecuritySchemeType.OAuth2,
                Description = "OAuth2 Description",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Flows = flows,
                Scheme = OAuthSettings.SchemeName

            };
            var securityrRequirements = new OpenApiSecurityRequirement();
            securityrRequirements.Add(oauthScheme, new List<string>() { "Bearer"});
            swaggerDoc.SecurityRequirements.Add(securityrRequirements);
        }
    }
}
