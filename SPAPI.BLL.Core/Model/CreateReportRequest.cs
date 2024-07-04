using System;
using System.Collections.Generic;

namespace SPAPI.BLL.Core.Model
{
    public class CreateReportRequest
    {
        public string reportType { get; set; }
        public string dataStartTime { get; set; }
        public string dataEndTime { get; set; }
        public List<string> marketplaceIds { get; set; }

        public CreateReportRequest()
        {
            marketplaceIds = new List<string>();
        }
    }
}
