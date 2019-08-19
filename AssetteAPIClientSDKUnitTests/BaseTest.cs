
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
                        ClientSecret = "3D5AC9A019200A5A4D0F0D8A38C8CAB0A8BFF5DC894FBCE0EE18B9B138937A3BF1A9AEAEC8E779A8384990B4D5DD2C7FA5F89D0E5FBB9DFDA1311BDA73A62EE3",
                        UserName = @"ADDN\assetteuser",
                        Password = "assette2client"
                    }
                };
                //_client = new AssetteApiClient(_assetteApiSettings) as IClient;
                return new AssetteApiClient(_assetteApiSettings);//Uncomment For Assette API

        }
        private async Task<IClient> InitAddendaSDKClient()
        {    var authSettings = new OktaSettings()
                {
                    ClientId = "0oauaatqnDFhZFgkp356",
                    ClientSecret = "wHWb6jbjEXP21Y0pJ6JvuYejBDmLL13CE4qHSkV8",
                    TokenUrl = "https://dev-193568.okta.com/oauth2/default/v1/token"
                };
                var manager = new PortalClientSDKManager();
                //var aToken = "eyJraWQiOiJINlZwVVA1YmFXQzdNcFRFX2VycmFBZ2ZmRnliRzNyQTZlS2NQbVhYc1dzIiwiYWxnIjoiUlMyNTYifQ.eyJ2ZXIiOjEsImp0aSI6IkFULlliM1hQY2pRNVlnS2J4aUZqZTVfZUloZUNqYjJVRk9wSVBiTWJKS3JBZGMiLCJpc3MiOiJodHRwczovL2Rldi0xOTM1Njgub2t0YS5jb20vb2F1dGgyL2RlZmF1bHQiLCJhdWQiOiJhcGk6Ly9kZWZhdWx0IiwiaWF0IjoxNTY0NTExNTM3LCJleHAiOjE1NjQ1MTUxMzcsImNpZCI6IjBvYXVhYXRxbkRGaFpGZ2twMzU2Iiwic2NwIjpbImN1c3RvbV9zY29wZSJdLCJzdWIiOiIwb2F1YWF0cW5ERmhaRmdrcDM1NiJ9.Frs9wv4dlKm89iIDzGGleoMmYcpSplLgPCf70cRUtW1EPB34Why2TEPyFeYpZpTP_rzY8zc9OFVWHLObwX3DsoakHeqaHwAQCmpDapPfGAnsOKMHHe9b9c6KOiELWGwQJ6q0jpM7FYpEHQuDbusvwTuzfhohwK-CeQP4suwREitrvy0GAaGEHslXguIG7ihczEgh2s0k39kkb7vRma3Gzec9vJ2Jx7syX5XpRn-rTU3OLkr6tSYHvxqhfs0HiYCa39GxodGi8xVHj8GfdPN8HJXDWHV0vMGz0m5g0znSkyVH5mWyk-mmy4Z4bSabes-mcqTjfqSR99t_Zn0lqnB_VA";
                return await manager.GetAddendaClient(authSettings, "https://localhost:44324/");//Uncomment for Addenda API
           

        }

    }
}