using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using SPAPI.BLL.Core.Model;
using SPAPI.BLL.Core.Signers;

namespace SPAPI.BLL.Core.Feeds
{
    public class GetFeedDocumentResult
    {
        ILog log = LogManager.GetLogger("GetFeedDocumentResult");

        private readonly string host;
        private const string getFeedDocumentRetriveResource = "feeds/2021-06-30/documents/{0}";
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        private const string JsonMediaType = "application/json; charset=utf-8";
        private readonly string region;
        private readonly string accessToken;

        public GetFeedDocumentResult(string _region)
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

        public Stream GetFile(DownloadFeedResultRequest downloadFeedResultRequest)
        {
            var endpoint = Path.Combine(host, String.Format(
                getFeedDocumentRetriveResource,               
                downloadFeedResultRequest.ResultFeedDocumentId
                ));

            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);

            request.AddHeader("x-amz-access-token", accessToken);
            request.AddHeader("X-Amz-Date", dateTimeStamp);

            var headers = new Dictionary<string, string>()
            {
                {"x-amz-access-token", accessToken}
            };

            var signer = new AWS4SignerForAuthorizationHeader
            {
                EndpointUri = new Uri(endpoint),
                HttpMethod = "GET",
                Service = "execute-api",
                Region = region
            };

            var authorization = signer.ComputeSignature(headers,
                                                        "",   // no query parameters
                                                        AWS4SignerBase.EMPTY_BODY_SHA256,
                                                        ConfigurationManager.AppSettings["AccessKey"].ToString(),
                                                        ConfigurationManager.AppSettings["SecretKey"].ToString());

            // place the computed signature into a formatted 'Authorization' header            
            request.AddHeader("Authorization", authorization);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<DownloadFeedResultResponse>(response.Content);

                if (responseObject == null )
                {
                    return null; //bad Request
                }

              return  SPAPIUtility.DownloadAndDecrypt(responseObject.url);
            }
            else
            {
                log.Error(response.Content);
                return null;
            }

            return null;
        }
     }
}
