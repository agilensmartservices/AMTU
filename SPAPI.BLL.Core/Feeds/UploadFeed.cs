using SPAPI.BLL.Core.Model;
using RestSharp;
using System;
using System.Configuration;
using System.IO;
using System.Net;


namespace SPAPI.BLL.Core.Feeds
{
    public class UploadFeed
    {
        public bool UploadObject(CreateFeedDocumentResponse uploadFeedRequest, string fileName, string countryCode)
        {
            WebRequest httpRequest = WebRequest.Create(uploadFeedRequest.url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "text/tab-separated-values; charset=UTF-8";
                     
            var fileContent = File.ReadAllText(fileName);

           // SPAPIUtility.RenameFile(fileName,"", countryCode, "_processing.txt");

            //if (uploadFeedRequest.payload != null && uploadFeedRequest.payload.encryptionDetails != null)
            //{
            //    var encryptedContent = new CryptoUtility().Encrypt(fileContent,
            //         Convert.FromBase64String(uploadFeedRequest.payload.encryptionDetails.key),
            //         Convert.FromBase64String(uploadFeedRequest.payload.encryptionDetails.initializationVector));

            //    return UploadFile(encryptedContent, uploadFeedRequest.payload.url);
            //}
            //else
            //{
            //    return UploadFile(fileContent, uploadFeedRequest.payload.url);
            //}

            return UploadFile(fileContent, uploadFeedRequest.url);
        }

        public bool UploadFile(string content, string url)
        {
            var contentType = "text/tab-separated-values; charset=UTF-8";

            var restClient = new RestClient(url);
            var restRequest = new RestRequest(Method.PUT)
              .AddParameter(contentType, content, ParameterType.RequestBody);

            var response = restClient.Execute(restRequest);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
                //Log Error
            }
        }
        public bool UploadFile(byte[] bytes, string url)
        {
            var contentType = "text/tab-separated-values; charset=UTF-8";
          
            var restClient = new RestClient(url);
            var restRequest = new RestRequest(Method.PUT)
                .AddParameter(contentType, bytes, ParameterType.RequestBody);

            var response = restClient.Execute(restRequest);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
                //Log Error
            }
        }
    }
}
