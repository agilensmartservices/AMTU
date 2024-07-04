using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAPI.BLL.Core.Model
{
    public class GetFeedResponse
    {
        public Feed payload { get; set; }
    }

    public class Feed
    {
        //The identifier for the feed. This identifier is unique only in combination with a seller ID.
        public string feedId { get; set; }

        //The feed type.
        public string feedType { get; set; }

        //A list of identifiers for the marketplaces that the feed is applied to.
        public List<string> marketplaceIds { get; set; }

        //The date and time when the feed was created, in ISO 8601 date time format.
        public string createdTime { get; set; }

        //The processing status of the feed.
        public string processingStatus { get; set; }

        //The date and time when feed processing started, in ISO 8601 date time format.
        public string processingStartTime { get; set; }

        //The date and time when feed processing completed, in ISO 8601 date time format.
        public string processingEndTime { get; set; }

        //The identifier for the feed document. This identifier is unique only in combination with a seller ID.
        public string resultFeedDocumentId { get; set; }
    }
}
