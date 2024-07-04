using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAPI.BLL.Core.Model
{
    public class FeedStatusResponse
    {
        public string feedId { get; set; }
        public string feedType { get; set; }
        public List<string> marketplaceIds { get; set; }
        public string createdTime { get; set; }
        public SPProcessingStatus processingStatus { get; set; }
        public string processingStartTime { get; set; }
        public string processingEndTime { get; set; }
        public string resultFeedDocumentId { get; set; }
    }

    //public class SPGetFeedResponsePayload
    //{
    //    public string feedId { get; set; }
    //    public string feedType { get; set; }
    //    public List<string> marketplaceIds { get; set; }
    //    public string createdTime { get; set; }
    //    public SPProcessingStatus processingStatus { get; set; }
    //    public string processingStartTime { get; set; }
    //    public string processingEndTime { get; set; }
    //    public string resultFeedDocumentId { get; set; }
    //}

    public enum SPProcessingStatus
    {
        CANCELLED,
        DONE,
        FATAL,
        IN_PROGRESS,
        IN_QUEUE
    }
}
