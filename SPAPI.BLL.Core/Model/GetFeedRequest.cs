using System;
using System.ComponentModel.DataAnnotations;

namespace SPAPI.BLL.Core.Model
{
    public class GetFeedRequest
    {
        [Required]
        public string feedId { get; set; }
    }
}
