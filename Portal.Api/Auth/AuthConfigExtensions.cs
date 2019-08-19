using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Portal.Api.Auth
{
    public static class CustomAuthExtensions
    {
        public static void AddApiKeyAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("ApiKey").AddScheme<AuthenticationSchemeOptions, ApiKeyAuth>("ApiKey", null);
        }
        //public static void AddOAuthAuthentication(this IServiceCollection services)
        //{
        //    services.AddAuthentication(OAuthSettings.SchemeName)
        //                        .AddScheme<OAuthOptions, AssetteoAuth2CCScheme>(OAuthSettings.SchemeName,
        //                        (options) =>
        //                        {
        //                            options.ClientId = OAuthSettings.ClientId;
        //                            options.ClientSecret = OAuthSettings.ClientSecret;
        //                           // options.AuthorizationEndpoint = new PathString(OAuthSettings.AuthorizationUrl);
        //                            options.TokenEndpoint = new PathString(OAuthSettings.TokenUrl);
        //                            //options.CallbackPath = new PathString(OAuthSettings.CallBackUrl);

        //                            //options.Scope.Add("api://43787b4a-7e04-4965-a1dd-0562c1dda7f2/AuthenticateUserScope");
        //                            OAuthSettings.Scopes.Keys.ToList().ForEach(x => options.Scope.Add(x));
        //                            options.Events = new OAuthEvents()
        //                            {
        //                                OnCreatingTicket = async context =>
        //                                {
        //                                    var request = new HttpRequestMessage()
        //                                    {
        //                                        RequestUri = new Uri(context.Options.UserInformationEndpoint),
        //                                        Method = HttpMethod.Get
        //                                    };
        //                                    request.Headers.Add(OAuthSettings.AuthHeaderName, "9aO7WGceZ3MGS4cAlOGjHd-uoeVfU/?.");
        //                                    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
        //                                    response.EnsureSuccessStatusCode();
        //                                },
        //                                OnRemoteFailure = context =>
        //                                {
        //                                    context.HandleResponse();
        //                                    context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
        //                                    return Task.FromResult(0);
        //                                }
        //                            };
        //                        });
        //}
        public static AuthenticationBuilder AddOktaAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication(
                                 (options) =>
                                 {
                                     options.DefaultScheme = OAuthSettings.TokenAuthenticationScheme;
                                     options.DefaultChallengeScheme = OAuthSettings.TokenAuthenticationScheme;
                                     options.DefaultAuthenticateScheme = OAuthSettings.TokenAuthenticationScheme;

                                 })
                                 .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(OAuthSettings.TokenAuthenticationScheme, 
                                             options =>{
                                                 options.ValidAudiences = OAuthSettings.Audiences;
                                                 options.ValidIssuer= OAuthSettings.IssuerURI;
                                             });
        }
        
    }
    public static class CustomOpenApiExtensions
    {
        public static void AddApiKeySecurity(this Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions setup)
        {
            setup.AddSecurityDefinition(OAuthSettings.AuthHeaderName, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = OAuthSettings.AuthHeaderName,
                In = ParameterLocation.Header,
                Description = "Please enter the API Key provided to you"
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = OAuthSettings.AuthHeaderName}
                        }, new List<string>() }
                });
        }
        public static void AddOAuthSecurity(this Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions setup)
        {
            var flows = new OpenApiOAuthFlows();
            flows.ClientCredentials = new OpenApiOAuthFlow()
            {
                TokenUrl = new Uri(OAuthSettings.TokenUrl, UriKind.Relative),
                Scopes = OAuthSettings.Scopes
            };
            var oauthScheme = new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Description = "OAuth2 Description",
                Name = OAuthSettings.AuthHeaderName,
                In = ParameterLocation.Query,
                Flows = flows,
                Scheme = OAuthSettings.SchemeName,

            };
            //securityrDefinition
            setup.AddSecurityDefinition("Bearer", oauthScheme);

            //securityrRequirements
            var securityrRequirements = new OpenApiSecurityRequirement();
            securityrRequirements.Add(oauthScheme, new List<string>() { });
            setup.AddSecurityRequirement(securityrRequirements);
        }
    }
}
