using System;
using System.Collections.Generic;

namespace SPAPI.BLL.Core.Model
{
    public class CreateFeedResponse
    {
        //The identifier for the feed. This identifier is unique only in combination with a seller ID.
        public string feedId { get; set; }
    }

    public class CreateFeedResult
    {
        public string feedId { get; set; }
    }
}
