using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.IO;

namespace SPAPI.BLL.Core
{
    public class RefreshToken
    {
        ILog log = LogManager.GetLogger("RefreshToken");
        //45 mins. the actual access token expiry time is 60 mins
        private readonly int TokenExpiresInSec = 2700; 
        private readonly string GrantTypeKey = "refresh_token";
        private readonly string JsonMediaType = "application/json; charset=utf-8";
        private readonly string AuthResource = "auth/o2/token";
      

        public RefreshToken()
        {            
        }

        private ICacheProvider _cacheProvider = null;

        private ICacheProvider cacheProvider
        {
            get { return _cacheProvider ?? (_cacheProvider = 
                    new DefaultCacheProvider(TokenExpiresInSec));
            }
        }

        private string GetRefreshToken(string region)
        {
            if(region.ToLower().Equals("us-east-1"))
            {
                return ConfigurationManager.AppSettings["RefreshToken_NA"].ToString();
            }
            else if (region.ToLower().Equals("eu-west-1"))
            {
                return ConfigurationManager.AppSettings["RefreshToken_EU"].ToString();
            }
            else if (region.ToLower().Equals("us-west-2"))
            {
                return ConfigurationManager.AppSettings["RefreshToken_ForEast"].ToString();
            }
            else
            {
                return "";
            }
        }

        public GetAccessTokenResponse GetAccessToken(string region)
        {
            string key = region+"_Token";
            var accessTokenInfo = cacheProvider.Get(key) as GetAccessTokenResponse;

            if (accessTokenInfo == null)
            {
                var host = ConfigurationManager.AppSettings["AmazonAuthAPIHost"].ToString(); 

                if (String.IsNullOrEmpty(host))
                    return null;

                var endpoint = Path.Combine(host, AuthResource);

                var client = new RestClient(endpoint);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                var requestParameters = new GetAccessTokenRequest()
                {
                    grant_type = GrantTypeKey,
                    refresh_token = GetRefreshToken(region),
                    client_id = ConfigurationManager.AppSettings["ClientId"].ToString(),
                    client_secret = ConfigurationManager.AppSettings["ClientSecret"].ToString()
                };

                var jsonRequestBody = JsonConvert.SerializeObject(requestParameters);

                request.AddParameter(JsonMediaType, jsonRequestBody, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {                    
                    var responseObject = JsonConvert.DeserializeObject<GetAccessTokenResponse>(response.Content);

                    cacheProvider.Set(key, responseObject);

                    return responseObject;
                }
                else
                {
                    log.Error(response.ErrorMessage);
                }               
            }
            return accessTokenInfo;
        }      
       
    }

    public class GetAccessTokenRequest
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }

        public GetAccessTokenRequest() { }
    }

    public class GetAccessTokenResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }

        public GetAccessTokenResponse() { }
    }
}
