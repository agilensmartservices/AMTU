using System;

namespace SPAPI.BLL.Core.Model
{
    public class CreateReportResponse
    {
        public Payload payload { get; set; }
        public string message { get; set; }

        public CreateReportResponse()
        {
            payload = new Payload();
        }
    }

    public class Payload
    {
        public string reportId { get; set; }
    }
}
