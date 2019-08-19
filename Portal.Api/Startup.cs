using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Portal.Api.Auth;
using Portal.Api.Repositories.Repos;
using Portal.Api.Filters;
using Portal.Api.Repositories;
using Microsoft.Data.Sqlite;
using AutoMapper;
using Portal.Api.Repositories.Profiles;
using AuthenticationServiceProxy;
using Portal.Api.Repositories.Contracts;
using Portal.Api.Repositories.InMemoryRepos;
using Portal.Api.Repositories.Repositories.AssociationRepo;
using Assette.Client;
using Sieve.Services;
using Portal.Api.Repositories.Sieve;
using Portal.Api.Repositories.Sieve_search;
using Sieve.Models;
using Microsoft.Extensions.Options;

namespace Portal.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddOktaAuthentication(); //Uncomment to enable Auth



            services.AddMvc((setupAction =>
            {
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(new ProducesDefaultResponseTypeAttribute());
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
               // setupAction.Filters.Add(new AuthorizeFilter());//Uncomment to Enable auth
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                var jsonOutputFormatter = setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    //remove text/json, the standard media type for working wih JSON at API level is application/json
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            }))
                .AddRazorPagesOptions(opts => opts.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext = actionContext as ActionExecutingContext;

                    //When validation errors
                    //TODO Oualid: verify action descriptor use
                    if (actionContext.ModelState.ErrorCount > 0 && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new BadRequestObjectResult(actionContext.ModelState);
                    }
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });
            services.AddVersionedApiExplorer();
            services.AddApiVersioning(cfg =>
            {
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen(setup =>
            {
                setup.DocInclusionPredicate((version, apiDescription) =>
                {
                    decimal versionMajor = 1;
                    decimal.TryParse(version, out versionMajor);
                    var major = Math.Truncate(versionMajor);

                    var values = apiDescription.RelativePath.Split('/').Skip(2);

                    apiDescription.RelativePath = $"api/v{major}/{string.Join("/", values)}";

                    var versionParam = apiDescription.ParameterDescriptions.SingleOrDefault(p => p.Name == "version");
                    if (versionParam != null)
                    {
                        apiDescription.ParameterDescriptions.Remove(versionParam);
                    }
                    return true;
                });
                #region Auth2 filters and Security
                setup.SchemaFilter<SchemaFilter>();

                setup.MapType<FileContentResult>(() => new OpenApiSchema { Type = "string", Format = "binary" });
                setup.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
                //setup.DocumentFilter<SecurityRequirementDocumentFilter>();
               setup.AddApiKeySecurity();//Uncomment to enableAuth

                #endregion




                setup.EnableAnnotations();
                setup.ResolveConflictingActions(api => api.First());
                var descriptionProvider =
                      services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
                foreach (var description in descriptionProvider.ApiVersionDescriptions)
                {
                    setup.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"Portal Api Specification {description.ApiVersion.ToString()}",
                        Description = "Api specification",
                        Version = description.ApiVersion.ToString(),
                        Contact = new OpenApiContact()
                        {
                            Email = "o.ktata@addendacapital.com",
                            Name = "Oualid Ktata"
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "Assette license",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                    setup.OperationFilter<FileUploadOperation>();
                    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    setup.IncludeXmlComments(xmlCommentsFullPath);
                }

            });

             var dbName = $"portalDb_{Environment.EnvironmentName}.db";

            //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database");
            // Specify that we will use sqlite and the path of the database here
            var sqlConnection = new SqliteConnection();
            // SqliteConnection.CreateFile("portal.sqlite");
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }
            var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = dbName }.ToString());
            var dbContextOptions = new DbContextOptionsBuilder<PortalDbContext>().UseSqlite(connection).Options;
            //using (var dbContext = new Repositories.PortalDbContext(dbContextOptions))
            //{
            //    dbContext.Database.EnsureCreated();
            //    if (!dbContext.Users.Any())
            //    {
            //        dbContext.Users.AddRange(new UserDto[] { });
            //    }
            //}
            services.AddMemoryCache();
            services.AddScoped<IRepoOptions, RepoOptions>();
            //Setup Account repos 
            services.AddScoped(typeof(ICachebaleRepository<AccountDto, AccountToCreateDto, AccountSimpleDto>), typeof(CachebaleRepository<AccountDto, AccountToCreateDto, AccountSimpleDto>));
            services.AddScoped<ICachebaleAccountRepository, InMemoryAccountRepository>();
            services.AddScoped<IAccountRepoOptions, AccountRepoOptions>(s => new AccountRepoOptions() { CacheItemName = "Account" });


            //Setup User repos
            services.AddScoped(typeof(ICachebaleRepository<UserDto, UserToCreateDto, UserSimpleDto>), typeof(CachebaleRepository<UserDto, UserToCreateDto, UserSimpleDto>));
            services.AddScoped<ICachebaleUserRepository, InMemoryUserRepository>();
            services.AddScoped<IUserRepoOptions, UserRepoOptions>(s => new UserRepoOptions() { CacheItemName = "User" });
            //Setup Document metadata repos
            services.AddScoped(typeof(ICachebaleRepository<DocumentDto, DocumentDto, DocumentSimpleDto>), typeof(CachebaleRepository<DocumentDto, DocumentDto, DocumentSimpleDto>));
            services.AddScoped<ICachebaleDocMetaDataRepository, InMemoryDocMetaDataRepository>();
            services.AddScoped<IDocMetaDataRepoOptions, DocMetaDataRepoOptions>(s => new DocMetaDataRepoOptions() { CacheItemName = "DocumentMetaData" });
            //Setup Associations repos
            services.AddScoped(typeof(ICachebaleRepository<AssociationDto, AssociationDto, AssociationSimpleDto>), typeof(CachebaleRepository<AssociationDto, AssociationDto, AssociationSimpleDto>));
            services.AddScoped<ICachebaleAssociationRepository, InMemoryAssociationRepository>();
            services.AddScoped<IAssociationRepoOptions, AssociationRepoOptions>(s => new AssociationRepoOptions() { CacheItemName = "Association" });
            //Setup Category repos
            services.AddScoped(typeof(ICachebaleRepository<CategoryDto, CategoryToCreateDto, CategoryDto>), typeof(CachebaleRepository<CategoryDto, CategoryToCreateDto, CategoryDto>));
            services.AddScoped<ICachebaleCategoryRepository, InMemoryCategoryRepository>();
            services.AddScoped<ICategoryRepoOptions, CategoryRepoOptions>(s => new CategoryRepoOptions() { CacheItemName = "Category" });

            //Setup DocumentType repos 
            services.AddScoped(typeof(ICachebaleRepository<DocumentTypeDto, DocumentTypeToCreateDto, DocumentTypeDto>), typeof(CachebaleRepository<DocumentTypeDto, DocumentTypeToCreateDto, DocumentTypeDto>));
            services.AddScoped<ICachebaleDocumentTypeRepository, InMemoryDocumentTypeRepository>();
            services.AddScoped<IDocumentTypeRepoOptions, DocumentTypeRepoOptions>(s => new DocumentTypeRepoOptions() { CacheItemName = "DocumentType" });

           
            services.AddAutoMapper(typeof(MappingProfiles));
            //services.Configure<SieveOptions>(new SieveO);
            services.AddScoped<IOptions<SieveOptions>, SieveCustomOptions>();
            services.AddScoped<ISieveProcessor,ApplicationSieveProcessor>();
            services.AddScoped <ISieveCustomFilterMethods,SieveCustomFilterMethods>();
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>();
            //DI for custom filters and sorts if needed

            //TODO: Get from Config after test
            services.AddScoped<ITokenService>(service => new OktaTokenService(new OktaSettings()
            {
                ClientSecret = OAuthSettings.ClientSecret,
                ClientId = OAuthSettings.ClientId,
                TokenUrl = OAuthSettings.OktaTokenUrl
            }));
            //Test DI
            //var provider = services.BuildServiceProvider();
            //var repoOption = provider.GetService<IRepoOptions>();
            //var userRepoOptions = provider.GetService<IUserRepoOptions>();
            //var caheRepo = provider.GetService<ICachebaleUserRepository>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePages();//To show this on browser
            app.UseHttpsRedirection();
            app.UseSwagger();

            apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(setup =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setup.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Assette Portal API Specification{description.ApiVersion.ToString()}");

                }
                setup.RoutePrefix = "";

                #region oAuth2
                //setup.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                //setup.OAuth2RedirectUrl(OAuthSettings.ClientId);
                setup.OAuthClientId(OAuthSettings.ClientId);
                setup.OAuthClientSecret(OAuthSettings.ClientSecret);
                setup.OAuthAppName("Portal Api");
                //TODO: remove additional querystring, testing purposes
                //var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{OAuthSettings.ClientId}:{OAuthSettings.ClientSecret}");
                //setup.OAuthAdditionalQueryStringParams( new Dictionary<string, string> { { OAuthSettings.AuthHeaderName,$"Basic {System.Convert.ToBase64String(clientCreds)}" } } );
                //setup.OAuth2RedirectUrl(new Uri(OAuthSettings.CallBackUrl, UriKind.Relative).ToString());
                //setup.OAuthAdditionalQueryStringParams( new Dictionary<string, string> { { "client_id",OAuthSettings.ClientId } } );

                #endregion
                setup.DefaultModelExpandDepth(2);
                setup.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setup.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                setup.EnableDeepLinking();
                setup.DisplayOperationId();
            });
            app.UseStaticFiles();
            //app.UseAuthentication();//Uncomment to enable auth
            app.UseMvc();
        }


    }
}
