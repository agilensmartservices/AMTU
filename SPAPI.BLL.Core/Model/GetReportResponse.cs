using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAPI.BLL.Core.Model
{
    public class GetReportResponse
    {
        //The payload for the getReport operation.
        public Report payload { get; set; }

        //A list of error responses returned when a request is unsuccessful.
        public string message { get; set; }

        public GetReportResponse()
        {

        }
    }
}
