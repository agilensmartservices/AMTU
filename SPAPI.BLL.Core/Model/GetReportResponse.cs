using System.Collections.Generic;

namespace SPAPI.BLL.Core.Model
{
    public class GetReportResponse
    {
        //A list of marketplace identifiers for the report.
        public List<string> marketplaceIds { get; set; }

        //required - The identifier for the report. This identifier is unique only in combination with a seller ID.
        public string reportId { get; set; }

        //required - The report type.
        public string reportType { get; set; }

        //The start of a date and time range used for selecting the data to report.
        public string dataStartTime { get; set; }

        //The end of a date and time range used for selecting the data to report.
        public string dataEndTime { get; set; }

        /*The identifier of the report schedule that created this report (if any). 
        This identifier is unique only in combination with a seller ID.*/
        public string reportScheduleId { get; set; }

        //required - The date and time when the report was created.
        public string createdTime { get; set; }

        //required - The processing status of the report.
        public string processingStatus { get; set; }

        //The date and time when the report processing started, in ISO 8601 date time format.
        public string processingStartTime { get; set; }

        //The date and time when the report processing completed, in ISO 8601 date time format.
        public string processingEndTime { get; set; }

        /* The identifier for the report document. 
        Pass this into the getReportDocument operation to get the information you will need 
        to retrieve and decrypt the report document's contents.*/
        public string reportDocumentId { get; set; }
    }
}
