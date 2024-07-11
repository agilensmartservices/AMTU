using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SPAPI.BLL.Core
{
    public class SPAPIUtility
    {
        public static string getHostByRegion(string region)
        {
            if (region.ToLower().Equals("us-east-1"))
            {
                return "https://sellingpartnerapi-na.amazon.com/";
            }
            else if (region.ToLower().Equals("eu-west-1"))
            {
                return "https://sellingpartnerapi-eu.amazon.com/";
            }
            else if (region.ToLower().Equals("us-west-2"))
            {
                return "https://sellingpartnerapi-fe.amazon.com/";
            }
            else
            {
                return "";
            }
        }

        public static string GetFeedFolderPathToProcess(string feedtype, string countryCode)
        {
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathToProcess"].ToString(), feedtype, countryCode);

            var files = Directory.GetFiles(fullPath).Where(x => !x.Contains("_processed")).ToList();

            if (files != null && files.Count > 0)
            {
                return Path.Combine(fullPath, files[0]);
            }
            return "";
        }

        public static List<string> GetFeedsToProcess(string feedtype, string countryCode)
        {
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathToProcess"].ToString(), feedtype, countryCode);

            var files = Directory.GetFiles(fullPath).Where(x => !x.Contains("_processed")).ToList();

            var pendingFiles = new List<string>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (!file.ToLower().Contains("_processed") && !file.ToLower().Contains("_processing"))
                        pendingFiles.Add(Path.Combine(fullPath, file));
                }
            }
            return pendingFiles;
        }

        public static string GetFeedFolderPathToDownloadResult(string feedtype, string countryCode)
        {
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathProcessed"].ToString(), feedtype, countryCode);

            var files = Directory.GetFiles(fullPath).Where(x => !x.Contains("_processed")).ToList();

            if (files != null && files.Count > 0)
            {
                return Path.GetFileName(files[0]);
            }
            return "";
        }

        public static bool CreateFile(string feedId, string feedtype, string countryCode)
        {
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathProcessed"].ToString(), feedtype, countryCode);

            var filePath = Path.Combine(fullPath, feedId + ".txt");

            File.Create(filePath);

            return true;
        }

        public static bool CreateFile(string reportId, string fileContent, string reporttype, string countryCode)
        {
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["ReportsFilePath"].ToString(), reporttype, countryCode);

            var filePath = Path.Combine(fullPath, reportId + ".txt");

            File.WriteAllText(filePath, fileContent);

            return true;
        }

        public static string RenameFile(string filename, string feedtype, string countryCode, string text = "_processed.txt")
        {
            if (filename.Contains("_processing"))
            {
                File.Move(filename, filename.Replace("_processing.txt", text));
            }
            else
                File.Move(filename, filename.Replace(".txt", text));

            return filename.Replace("_processing.txt", text);
        }

        public static bool DeleteFile(string feedId, string feedtype, string countryCode)
        {
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathProcessed"].ToString(), feedtype, countryCode);

            var filePath = Path.Combine(fullPath, feedId + ".txt");

            File.Delete(filePath);

            return true;
        }
        public static string GetDirectoryByFeedType(string feedType)
        {
            if (feedType.ToUpper().Equals("POST_FLAT_FILE_INVLOADER_DATA"))
            {
                return "ListingLoader";
            }
            else if (feedType.ToUpper().Equals("POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA"))
            {
                return "PriceandStock";
            }
            else if (feedType.ToUpper().Equals("POST_FLAT_FILE_FULFILLMENT_DATA"))
            {
                return "Shipment";
            }
            return "";
        }

        public static Stream DownloadAndDecrypt(string url)
        {
            byte[] cipherText = DownloadFile(url);
            if (cipherText == null) return null;
            //if (retriveInfo.encryptionDetails.standard == "AES")
            //{
            //    string data = CryptoUtility.Decrypt(cipherText,
            //        Convert.FromBase64String(retriveInfo.encryptionDetails.key),
            //        Convert.FromBase64String(retriveInfo.encryptionDetails.initializationVector));
            //    return new MemoryStream(Encoding.UTF8.GetBytes(data));
            //}

            return new MemoryStream(cipherText);
        }
        public static byte[] DownloadFile(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", Method.GET);
            var response = client.DownloadData(request);
            return response;
        }

        public static void CopyStream(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }

    }
}

