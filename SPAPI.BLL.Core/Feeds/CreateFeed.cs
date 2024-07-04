using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using SPAPI.BLL.Core.Model;
using SPAPI.BLL.Core.Signers;

namespace SPAPI.BLL.Core.Feeds
{
    public class CreateFeed
    {
        ILog log = LogManager.GetLogger("Feed");

        private readonly string host;
        private const string createFeedResource = "feeds/2021-06-30/feeds";
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        private const string JsonMediaType = "application/json; charset=utf-8";
        private readonly string region;
        private readonly string accessToken;

        public CreateFeed(string _region)
        {
            var accessTokenInfo = new RefreshToken().GetAccessToken(_region);

            if (accessTokenInfo == null)
            {
                log.Error("Invalid Access Token or the value is NULL");
            }
            else
            {
                accessToken = accessTokenInfo.access_token;
            }
            region = _region;

            host = SPAPIUtility.getHostByRegion(region);
        }

        public CreateFeedResponse Post(CreateFeedRequest createFeedRequest)
        {
            var endpoint = Path.Combine(host, createFeedResource);

            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);

            request.AddHeader("x-amz-access-token", accessToken);
            request.AddHeader("x-amz-date", dateTimeStamp);

            string requestJSON = JsonConvert.SerializeObject(createFeedRequest);

            // precompute hash of the body content
            var contentHash = AWS4SignerBase.CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(requestJSON));
            var contentHashString = AWS4SignerBase.ToHexString(contentHash, true);

            var headers = new Dictionary<string, string>()
            {
                {"x-amz-access-token", accessToken},
                {"x-amz-content-sha256", contentHashString}
            };

            request.AddHeader("x-amz-content-sha256", contentHashString);

            var signer = new AWS4SignerForAuthorizationHeader
            {
                EndpointUri = new Uri(endpoint),
                HttpMethod = "POST",
                Service = "execute-api",
                Region = region
            };

            var authorization = signer.ComputeSignature(headers,
                                                        "",   // no query parameters
                                                        contentHashString,
                                                         ConfigurationManager.AppSettings["AccessKey"].ToString(),
                                                        ConfigurationManager.AppSettings["SecretKey"].ToString());

            // place the computed signature into a formatted 'Authorization' header            
            request.AddHeader("Authorization", authorization);

            request.AddParameter("", requestJSON, JsonMediaType, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<CreateFeedResponse>(response.Content);

                return responseObject;
            }
            else
            {
                log.Error(response.Content);
                return null;
            }
        }
    }
}
