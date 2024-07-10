using log4net;
using Newtonsoft.Json;
using RestSharp;
using SPAPI.BLL.Core.Model;
using SPAPI.BLL.Core.Signers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace SPAPI.BLL.Core.Reports
{
    public class Reports
    {
        ILog log = LogManager.GetLogger("Reports");

        private readonly string CreateReportResource = "reports/2021-06-30/reports";
        private readonly string ReportsResource = "reports/2021-06-30/reports?reportTypes={0}";
        private readonly string ReportResource = "reports/2021-06-30/reports/{reportId}";
        private readonly string ReportDocumentResource = "reports/2021-06-30/documents/{reportDocumentId}";
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        private readonly string JsonMediaType = "application/json; charset=utf-8";
        private readonly string region;
        private readonly string accessToken;
        private readonly string host;

        public Reports(string _region)
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

        public CreateReportResponse createReport(CreateReportRequest createReportRequest)
        {
            if (String.IsNullOrEmpty(accessToken))
            {
                log.ErrorFormat("Invalid refresh token for region {0}", region);
                return null;
            }

            var endpoint = Path.Combine(host, CreateReportResource);

            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);

            request.AddHeader("x-amz-access-token", accessToken);
            request.AddHeader("X-Amz-Date", dateTimeStamp);

            string requestJSON = JsonConvert.SerializeObject(createReportRequest);
            // precompute hash of the body content
            var contentHash = AWS4SignerBase.CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(requestJSON));
            var contentHashString = AWS4SignerBase.ToHexString(contentHash, true);

            var headers = new Dictionary<string, string>()
            {
                {"x-amz-access-token", accessToken},
                {"x-amz-content-sha256", contentHashString}
            };

            request.AddHeader("X-Amz-Content-Sha256", contentHashString);

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

            request.AddParameter(JsonMediaType, requestJSON, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<CreateReportResponse>(response.Content);

                return responseObject;
            }
            else
            {
                log.Error(response.Content);
                return null;
            }
        }

        public GetReportsResponse getReports(GetReportsRequest getReportsRequest)
        {
            if (String.IsNullOrEmpty(host))
                return null;

            var endpoint = Path.Combine(host, String.Format(ReportsResource, getReportsRequest.reportTypes[0]));

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
                                                        "reportTypes=" + getReportsRequest.reportTypes[0],   // no query parameters
                                                        AWS4SignerBase.EMPTY_BODY_SHA256,
                                                        ConfigurationManager.AppSettings["AccessKey"].ToString(),
                                                        ConfigurationManager.AppSettings["SecretKey"].ToString());

            // place the computed signature into a formatted 'Authorization' header            
            request.AddHeader("Authorization", authorization);

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<GetReportsResponse>(response.Content);

                return responseObject;
            }
            else
            {
                log.Error(response.Content);

                return null;
            }
        }


        public GetReportResponse getReport(string reportId)
        {
            if (String.IsNullOrEmpty(host))
                return null;

            var endpoint = Path.Combine(host, ReportResource.Replace("{reportId}", reportId));

            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);

            request.AddHeader("x-amz-access-token", accessToken);
            request.AddHeader("x-amz-date", dateTimeStamp);

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
                                                        "",   // query parameters
                                                        AWS4SignerBase.EMPTY_BODY_SHA256,
                                                        ConfigurationManager.AppSettings["AccessKey"].ToString(),
                                                        ConfigurationManager.AppSettings["SecretKey"].ToString());

            // place the computed signature into a formatted 'Authorization' header            
            request.AddHeader("Authorization", authorization);

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<GetReportResponse>(response.Content);

                return responseObject;
            }
            else
            {
                log.Error(response.Content);
                return null;
            }
        }

        public GetReportDocumentResponse getReportDocument(string reportDocumentId)
        {

            if (String.IsNullOrEmpty(host))
                return null;

            var endpoint = Path.Combine(host, ReportDocumentResource.Replace("{reportDocumentId}", reportDocumentId));

            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);

            request.AddHeader("x-amz-access-token", accessToken);
            request.AddHeader("x-amz-date", dateTimeStamp);

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
                                                        "",   // query parameters
                                                        AWS4SignerBase.EMPTY_BODY_SHA256,
                                                        ConfigurationManager.AppSettings["AccessKey"].ToString(),
                                                        ConfigurationManager.AppSettings["SecretKey"].ToString());

            // place the computed signature into a formatted 'Authorization' header            
            request.AddHeader("Authorization", authorization);

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<GetReportDocumentResponse>(response.Content);

                return responseObject;
            }
            else
            {
                log.Error(response.Content);
                return null;
            }
        }

        public byte[] downloadReportByte(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }


    }
}
