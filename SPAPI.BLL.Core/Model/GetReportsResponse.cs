using System;
using System.Collections.Generic;

namespace SPAPI.BLL.Core.Model
{
    //The response for the getReports operation.
    public class GetReportsResponse
    {
        //The payload for the getReports operation.
        public List<Report> payload { get; set; }

        /*Returned when the number of results exceeds pageSize. 
        To get the next page of results, call getReports with this token as the only parameter. */
        public string nextToken { get; set; }

        //A list of error responses returned when a request is unsuccessful.
        public List<Error> errors { get; set; }

        public string message { get; set; }

    }

    public class Report
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

    public class Error
    {
        //required - An error code that identifies the type of error that occurred.
        public string code { get; set; }
        //required - A message that describes the error condition in a human-readable form
        public string message { get; set; }
        //Additional details that can help the caller understand or fix the issue.
        public string details { get; set; }
    }
}
