using System;
using System.ComponentModel.DataAnnotations;

namespace SPAPI.BLL.Core.Model
{
    public class CreateFeedDocumentRequest
    {
        [Required]
        public string contentType { get; set; }
    }
}
