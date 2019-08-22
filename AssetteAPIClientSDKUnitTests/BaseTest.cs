
using API.SDK;
using API.SDK.Contracts;
using API.SDK.Manager;
using Assette.Client;
using AuthenticationServiceProxy;
using System;
using System.Threading.Tasks;

namespace Portal.SDK.Test
{
    public class BaseTest
    {
        //protected Client _client { get; private set; }
        //protected AssetteApiClient _client { get; private set; }//Uncomment to use assette API
        protected IClient _client { get; private set; }//Uncomment to use Addenda API
        protected Generators _generators;
       
        public BaseTest() : base()
        {
            //InitSDKClient(TargetApi.Addenda).Wait();// Choose Assette Enum to run on Assette API
            //InitAssetteSDKClient().Wait();
            _client=InitAddendaSDKClient().Result;
            _generators = new Generators();
        }
        private async Task<AssetteApiClient> InitAssetteSDKClient()
        {
             AssetteApiSettings _assetteApiSettings = new AssetteApiSettings
                {
                    AuthUrl = @"https://app.stg.assette.com/services/id/auth/token",
                    BaseAddress = @"https://app.stg.assette.com/app/admin",
                    Credentials = new AssetteApiCredentials
                    {
                        ClientId = "ADDN",
                        ClientSecret = "SECRET",
                        UserName = @"SECRET",
                        Password = "SECRET"
                    }
                };
                //_client = new AssetteApiClient(_assetteApiSettings) as IClient;
                return new AssetteApiClient(_assetteApiSettings);//Uncomment For Assette API

        }
        private async Task<IClient> InitAddendaSDKClient()
        {    var authSettings = new OktaSettings()
                {
                    ClientId = "SECRET",
                    ClientSecret = "SECRET",
                    TokenUrl = "https://dev-SECRET.okta.com/oauth2/default/v1/token"
                };
                var manager = new PortalClientSDKManager();
                return await manager.GetAddendaClient(authSettings, "https://localhost:44324/");//Uncomment for Addenda API
           

        }

    }
}
