using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAPI.BLL.Core.Model
{
    public class GetReportsRequest
    {
        /*
         * A list of report types used to filter reports. When reportTypes is provided, the other filter parameters 
         * (processingStatuses, marketplaceIds, createdSince, createdUntil) and pageSize may also be provided. 
         * Either reportTypes or nextToken is required.
         * */
        public List<string> reportTypes { get; set; }

        //A list of processing statuses used to filter reports.
        public ProcessingStatuses processingStatuses { get; set; }

        /*A list of marketplace identifiers used to filter reports. 
        The reports returned will match at least one of the marketplaces that you specify. */
        public List<string> marketplaceIds { get; set; }

        //The maximum number of reports to return in a single call.
        public int pageSize { get; set; }

        /*
         * The earliest report creation date and time for reports to include in the response, in ISO 8601 date time format. 
         * The default is 90 days ago. Reports are retained for a maximum of 90 days.
         * */
        public string createdSince { get; set; }

        /*
         * The latest report creation date and time for reports to include in the response, in ISO 8601 date time format. 
         * The default is now.
         * */
        public string createdUntil { get; set; }

        /*
         A string token returned in the response to your previous request. 
         nextToken is returned when the number of results exceeds the specified pageSize value. 
         To get the next page of results, call the getReports operation and include this token as the only parameter.
         Specifying nextToken with any other parameters will cause the request to fail.
         */
        public string nextToken { get; set; }

        public GetReportsRequest() { }

    }

    public enum ProcessingStatuses
    {
        CANCELLED,
        DONE,
        FATAL,
        IN_PROGRESS,
        IN_QUEUE
    }
}
