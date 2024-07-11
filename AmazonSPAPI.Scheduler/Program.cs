using log4net;
using Newtonsoft.Json;
using SPAPI.BLL.Core;
using SPAPI.BLL.Core.Feeds;
using SPAPI.BLL.Core.Model;
using SPAPI.BLL.Core.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace AmazonSPAPI.Scheduler
{
    public class Program
    {
        static void Main(string[] args)
        {
            ProcessFeed();

            ProcessFeedResult();

            ProcessReports();

            Environment.Exit(0);
        }

        private static bool IsFeedExistToProcess(string feedType, string countryCode)
        {
            ILog log = LogManager.GetLogger("AMTU_Scheduler");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            try
            {
                var feedToProcess = SPAPIUtility.GetFeedFolderPathToProcess(feedType, countryCode);


                var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathToProcess"].ToString(), feedType, countryCode);

                log.InfoFormat(fullPath);

                if (String.IsNullOrEmpty(feedToProcess))
                {
                    log.InfoFormat("There is no {0} feed to process in {1} ", feedType, countryCode);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        private static bool IsFeedExistToDownloadResult(string feedType, string countryCode)
        {
            ILog log = LogManager.GetLogger("AMTU_Scheduler");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            try
            {
                var feedToProcess = SPAPIUtility.GetFeedFolderPathToDownloadResult(feedType, countryCode);

                if (String.IsNullOrEmpty(feedToProcess))
                {
                    log.InfoFormat("There is no {0} feed document to process in {1} ", feedType, countryCode);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public static void ProcessFeed()
        {
            ILog log = LogManager.GetLogger("Feed Process Initiated");

            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            var marketplaceIds_NA = ConfigurationManager.AppSettings["MarketplaceIds_NA"].Split(',');
            var marketplaceIds_EU = ConfigurationManager.AppSettings["MarketplaceIds_EU"].Split(',');
            var marketplaceIds_ForEast = ConfigurationManager.AppSettings["MarketplaceIds_ForEast"].Split(',');

            foreach (var marketplaceId in marketplaceIds_NA)
            {
                try
                {
                    var countryCode = marketplaceId.Split('|')[0].ToString();
                    var marketplace = marketplaceId.Split('|')[1].ToString();

                    if (IsFeedExistToProcess("ListingLoader", countryCode))
                    {
                        Worker(marketplace, "us-east-1", "POST_FLAT_FILE_INVLOADER_DATA", countryCode);
                    }
                    if (IsFeedExistToProcess("PriceandStock", countryCode))
                    {
                        Worker(marketplace, "us-east-1", "POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA", countryCode);
                    }
                    if (IsFeedExistToProcess("Shipment", countryCode))
                    {
                        Worker(marketplace, "us-east-1", "POST_FLAT_FILE_FULFILLMENT_DATA", countryCode);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    continue;
                }
            }

            foreach (var marketplaceId in marketplaceIds_EU)
            {
                try
                {
                    var countryCode = marketplaceId.Split('|')[0].ToString();
                    var marketplace = marketplaceId.Split('|')[1].ToString();

                    log.Error(countryCode);

                    if (IsFeedExistToProcess("ListingLoader", countryCode))
                    {
                        Worker(marketplace, "eu-west-1", "POST_FLAT_FILE_INVLOADER_DATA", countryCode);
                    }
                    if (IsFeedExistToProcess("PriceandStock", countryCode))
                    {
                        Worker(marketplace, "eu-west-1", "POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA", countryCode);
                    }
                    if (IsFeedExistToProcess("Shipment", countryCode))
                    {
                        Worker(marketplace, "eu-west-1", "POST_FLAT_FILE_FULFILLMENT_DATA", countryCode);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                    continue;
                }
            }

            foreach (var marketplaceId in marketplaceIds_ForEast)
            {
                try
                {
                    var countryCode = marketplaceId.Split('|')[0].ToString();
                    var marketplace = marketplaceId.Split('|')[1].ToString();

                    if (IsFeedExistToProcess("ListingLoader", countryCode))
                    {
                        Worker(marketplace, "us-west-2", "POST_FLAT_FILE_INVLOADER_DATA", countryCode);
                    }
                    if (IsFeedExistToProcess("PriceandStock", countryCode))
                    {
                        Worker(marketplace, "us-west-2", "POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA", countryCode);
                    }
                    if (IsFeedExistToProcess("Shipment", countryCode))
                    {
                        Worker(marketplace, "us-west-2", "POST_FLAT_FILE_FULFILLMENT_DATA", countryCode);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    continue;
                }
            }
        }

        public static void ProcessFeedResult()
        {
            ILog log = LogManager.GetLogger("Feed Process Initiated");

            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            var marketplaceIds_NA = ConfigurationManager.AppSettings["MarketplaceIds_NA"].Split(',');
            var marketplaceIds_EU = ConfigurationManager.AppSettings["MarketplaceIds_EU"].Split(',');
            var marketplaceIds_ForEast = ConfigurationManager.AppSettings["MarketplaceIds_ForEast"].Split(',');

            foreach (var marketplaceId in marketplaceIds_NA)
            {
                var countryCode = marketplaceId.Split('|')[0].ToString();
                var marketplace = marketplaceId.Split('|')[1].ToString();

                if (IsFeedExistToDownloadResult("ListingLoader", countryCode))
                {
                    ResultWorker(marketplace, "us-east-1", "POST_FLAT_FILE_INVLOADER_DATA", countryCode);
                }
                if (IsFeedExistToDownloadResult("PriceandStock", countryCode))
                {
                    ResultWorker(marketplace, "us-east-1", "POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA", countryCode);
                }
                if (IsFeedExistToDownloadResult("Shipment", countryCode))
                {
                    ResultWorker(marketplace, "us-east-1", "POST_FLAT_FILE_FULFILLMENT_DATA", countryCode);
                }
            }

            foreach (var marketplaceId in marketplaceIds_EU)
            {
                var countryCode = marketplaceId.Split('|')[0].ToString();
                var marketplace = marketplaceId.Split('|')[1].ToString();

                if (IsFeedExistToDownloadResult("ListingLoader", countryCode))
                {
                    ResultWorker(marketplace, "eu-west-1", "POST_FLAT_FILE_INVLOADER_DATA", countryCode);
                }
                if (IsFeedExistToDownloadResult("PriceandStock", countryCode))
                {
                    ResultWorker(marketplace, "eu-west-1", "POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA", countryCode);
                }
                if (IsFeedExistToDownloadResult("Shipment", countryCode))
                {
                    ResultWorker(marketplace, "eu-west-1", "POST_FLAT_FILE_FULFILLMENT_DATA", countryCode);
                }
            }

            foreach (var marketplaceId in marketplaceIds_ForEast)
            {
                var countryCode = marketplaceId.Split('|')[0].ToString();
                var marketplace = marketplaceId.Split('|')[1].ToString();

                if (IsFeedExistToDownloadResult("ListingLoader", countryCode))
                {
                    ResultWorker(marketplace, "us-west-2", "POST_FLAT_FILE_INVLOADER_DATA", countryCode);
                }
                if (IsFeedExistToDownloadResult("PriceandStock", countryCode))
                {
                    ResultWorker(marketplace, "us-west-2", "POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA", countryCode);
                }
                if (IsFeedExistToDownloadResult("Shipment", countryCode))
                {
                    ResultWorker(marketplace, "us-west-2", "POST_FLAT_FILE_FULFILLMENT_DATA", countryCode);
                }
            }
        }

        public static void ProcessReports()
        {
            ILog log = LogManager.GetLogger("OrderReports Download Initiated");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            var marketplaceIds_NA = ConfigurationManager.AppSettings["MarketplaceIds_NA"].Split(',');
            var marketplaceIds_EU = ConfigurationManager.AppSettings["MarketplaceIds_EU"].Split(',');
            var marketplaceIds_ForEast = ConfigurationManager.AppSettings["MarketplaceIds_ForEast"].Split(',');

            foreach (var marketplaceId in marketplaceIds_NA)
            {
                try
                {
                    var countryCode = marketplaceId.Split('|')[0].ToString();
                    var marketplace = marketplaceId.Split('|')[1].ToString();
                    log.InfoFormat("Download Orders Report for {0} Started", countryCode);
                    ReportWorker(marketplace, "us-east-1", "GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_GENERAL", countryCode);
                    log.InfoFormat("Download Orders Report for {0} Completed", countryCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    continue;
                }
            }

            foreach (var marketplaceId in marketplaceIds_EU)
            {
                try
                {
                    var countryCode = marketplaceId.Split('|')[0].ToString();
                    var marketplace = marketplaceId.Split('|')[1].ToString();

                    log.InfoFormat("Download Orders Report for {0} Started", countryCode);
                    ReportWorker(marketplace, "eu-west-1", "GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_GENERAL", countryCode);
                    log.InfoFormat("Download Orders Report for {0} Completed", countryCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    continue;
                }
            }

            foreach (var marketplaceId in marketplaceIds_ForEast)
            {
                try
                {
                    var countryCode = marketplaceId.Split('|')[0].ToString();
                    var marketplace = marketplaceId.Split('|')[1].ToString();

                    log.InfoFormat("Download Orders Report for {0} Started", countryCode);
                    ReportWorker(marketplace, "us-west-2", "GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_GENERAL", countryCode);
                    log.InfoFormat("Download Orders Report for {0} Completed", countryCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    continue;
                }
            }
        }

        public static void Worker(string marketplaceId, string region, string feedtype, string countryCode)
        {

            ILog log = LogManager.GetLogger("AMTU_Scheduler");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            try
            {
                log.InfoFormat("Started Feed Processing for type {0} marketplace {1}", feedtype, marketplaceId);

                log.Error(region);

                //Get pending files list form the directory
                var path = SPAPIUtility.GetDirectoryByFeedType(feedtype);
                log.InfoFormat(path);
                var feedsToProcess = SPAPIUtility.GetFeedsToProcess(path, countryCode);
                log.Error(feedsToProcess);
                string feedname = "";
                foreach (var feed in feedsToProcess)
                {
                    try
                    {
                        feedname = feed;
                        //1. Create Feed Document by Content type whcih we are going to upload
                        var response = new CreateFeedDocument(region).Post(new CreateFeedDocumentRequest()
                        {
                            contentType = "text/tab-separated-values; charset=UTF-8"
                        });

                        log.Info("*************Step 1: Feed Document: *********");

                        log.Info(JsonConvert.SerializeObject(response));

                        if (response == null)
                        {
                            log.Info("Invalid Feed Dcoument!");
                            return;
                        }

                        var feedDocumentId = response.feedDocumentId;

                        //2. Encrypt and Upload the feed Document                      

                        var uploaded = new UploadFeed().UploadObject(new CreateFeedDocumentResponse()
                        {
                            url = response.url,
                            feedDocumentId = response.feedDocumentId
                        }, feed, countryCode);

                        //3. Create Feed
                        if (uploaded)
                        {
                            log.Info("*************Step 2: UploadFeed Completed *********");

                            log.Info("*************Step 3: Create Feed *********");

                            var feedResponse = new CreateFeed(region).Post(new CreateFeedRequest()
                            {
                                feedType = feedtype,
                                inputFeedDocumentId = feedDocumentId,
                                marketplaceIds = new List<string>() { marketplaceId }
                            });

                            log.Info(JsonConvert.SerializeObject(feedResponse));
                            SPAPIUtility.CreateFile(feedResponse.feedId, "ListingLoader", countryCode);
                            SPAPIUtility.RenameFile(feed, path, countryCode);

                            //DB Update - FeedId
                            log.Info("DB Call Initiated");
                            //new SchedulerDAL().FeedIdUpdate(feedResponse.feedId, feed);
                        }
                        else
                        {
                            log.Info("Feed not uploaded!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.InfoFormat("Feed file {1} not uploaded for {0}!", countryCode, feedname);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("UK");
            }
            log.InfoFormat("***********Feed Process Completed for {0}***********************", marketplaceId);
        }

        public static void ResultWorker(string marketplaceId, string region, string feedtype, string countryCode)
        {
            ILog log = LogManager.GetLogger("AMTU_Scheduler_FeedResult");
            log4net.Config.XmlConfigurator.Configure(
                new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            try
            {
                var path = SPAPIUtility.GetDirectoryByFeedType(feedtype);
                var filename = SPAPIUtility.GetFeedFolderPathToDownloadResult(path, countryCode);

                var fullPath = Path.Combine(ConfigurationManager.AppSettings["FeedFilePathProcessed"].ToString(), path, countryCode);

                log.InfoFormat(fullPath);

                log.InfoFormat(countryCode);


                var documentId = filename.Replace(".txt", "");

                var resultDcoumentId = new GetFeed(region).Get(new DownloadFeedResultRequest()
                {
                    ResultFeedDocumentId = documentId
                });

                if (String.IsNullOrEmpty(resultDcoumentId))
                {
                    log.InfoFormat("Feed {0} is in progress", documentId);
                    return;
                }

                SPAPIUtility.RenameFile(Path.Combine(fullPath, filename), path, countryCode);

                //1. Create Feed Document by Content type whcih we are going to upload
                var response = new GetFeedDocumentResult(region).GetFile(new DownloadFeedResultRequest()
                {
                    ResultFeedDocumentId = resultDcoumentId
                });

                if (response == null)
                {
                    log.Info("Invalid FeedId");
                    return;
                }

                SPAPIUtility.CopyStream(response,
                    Path.Combine(
                        ConfigurationManager.AppSettings["FeedResultFilePath"].ToString(),
                        path,
                        countryCode,
                        documentId + ".txt"));



                //new SchedulerDAL().FeedResultUpdate(documentId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            log.InfoFormat("***********Download Feed Processing Result Process Completed for {0}***********************", marketplaceId);
        }

        public static void ReportWorker(string marketplaceId, string region, string reporttype, string countryCode)
        {
            ILog log = LogManager.GetLogger("OrderReports");

            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));


            //************************ Create Reports Starts ***********************************
            TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            int LastXHours = int.Parse(ConfigurationManager.AppSettings["OrdersCreatedInLastXHours"]);

            var requestDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddHours(-LastXHours), pacificZone);

            var startTime = requestDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            TimeSpan newTime = new TimeSpan(23, 59, 0);

            var endTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow + newTime, pacificZone).ToString("yyyy-MM-ddTHH:mm:ss");

            var reports = new Reports(region).createReport(new SPAPI.BLL.Core.Model.CreateReportRequest()
            {
                reportType = reporttype,
                dataStartTime = startTime,
                dataEndTime = endTime,
                marketplaceIds = new List<string>() { marketplaceId }
            });

            if (reports is null || string.IsNullOrEmpty(reports.reportId))
            {
                log.InfoFormat("Error occured while create Report.");
                return;
            }

            var reportId = reports.reportId;

            log.InfoFormat("ReportId:{0}", reportId);
        //************************ Create Reports Ends ***********************************
        tryAgain:
            var report = new Reports(region).getReport(reportId);

            //Check peroidically the status till move to DONE, to get reportDocumentId
            if (report.processingStatus == ProcessingStatuses.IN_QUEUE.ToString() ||
                report.processingStatus == ProcessingStatuses.IN_PROGRESS.ToString())
            {
                //TODO: Max wait time for report should be defined
                System.Threading.Thread.Sleep(1000 * 60 * 1); // 1 min wait time
                goto tryAgain;
            }
            else if (report.processingStatus == ProcessingStatuses.DONE.ToString())
            {
                //Download Report Document
                var reportDocument = new Reports(region).getReportDocument(report.reportDocumentId);

                if (reportDocument is null || string.IsNullOrEmpty(reportDocument.url) || string.IsNullOrEmpty(reportDocument.reportDocumentId))
                {
                    log.ErrorFormat("Error occurred while download report.");
                    return;
                }

                var downloadURL = reportDocument.url;

                var encryptedReportContent = new Reports(region).downloadReportByte(downloadURL);
                string plainReportContent = Encoding.UTF8.GetString(encryptedReportContent);

                ////Decrypt the report content
                //var plainReportContent = CryptoUtility.Decrypt
                //        (encryptedReportContent,
                //        Convert.FromBase64String(reportDocument.payload.encryptionDetails.key),
                //        Convert.FromBase64String(reportDocument.payload.encryptionDetails.initializationVector)
                //        );

                bool isCompressed = CryptoUtility.IsGzipCompressed(encryptedReportContent);

                if (reportDocument.compressionAlgorithm.ToString().ToLower().Equals("gzip") && isCompressed)
                {
                    var reportBytes = CryptoUtility.Decompress(encryptedReportContent);
                    plainReportContent = Encoding.UTF8.GetString(reportBytes);
                }

                SPAPIUtility.CreateFile(reportId, plainReportContent, "PendingOrders", countryCode);
            }

        }
    }
}
