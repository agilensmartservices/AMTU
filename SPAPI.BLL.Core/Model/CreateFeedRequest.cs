using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPAPI.BLL.Core.Model
{
    public class CreateFeedRequest
    {
        [Required]
        public string feedType { get; set; }
        [Required]
        public List<string> marketplaceIds { get; set; }
        [Required]
        public string inputFeedDocumentId { get; set; }
        public FeedOptions feedOptions { get; set; }
    }
   
    public class FeedOptions
    {
        [Required]
        public Dictionary<string, string> map { get; set; }
    }
}
