using API.SDK.Contracts;
using Assette.Client;
using AuthenticationServiceProxy;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.SDK.Manager
{
    public class PortalClientSDKManager
    {

        public async Task<IClient> GetAddendaClient(OktaSettings authSettings, string baseUri,string token=null)
        {
            try
            {
                if (token == null)
                {
                    var _authService = new OktaTokenService(authSettings);
                    token = await _authService.GetToken();
                    if (token == null)
                    {
                        throw new NullReferenceException("Could not Authenticate!");
                    }
                }
                
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return new Client(baseUri, httpClient);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IClient GetAssetteClient(AssetteApiSettings assetteApiSettings)
        {
            try
            {
                var _assetteApiClient = new AssetteApiClient(assetteApiSettings);
                return _assetteApiClient as IClient;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



    }
}
