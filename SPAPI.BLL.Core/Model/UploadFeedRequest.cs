using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPAPI.BLL.Core.Model
{
    public class UploadFeedRequest
    {
        [Required]
        public FeedInfo payload { get; set; }

        public class FeedInfo
        {
            public string feedDocumentId { get; set; }
            public EncryptionDetails encryptionDetails { get; set; }
            public string url { get; set; }

            public FeedInfo()
            {
                encryptionDetails = new EncryptionDetails();
            }

        }
    }
}
