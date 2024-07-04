using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAPI.BLL.Core.Model
{
    public class DownloadFeedResultResponse
    {
        public string feedDocumentId { get; set; }
        public string url { get; set; }
        public string compressionAlgorithm { get; set; }
    }

    public class SPError
    {
        public string code { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }
    public class GetFeedDocumentRetriveInfoPayload
    {
        public string feedDocumentId { get; set; }
        public string url { get; set; }
        public SPEncryptionDetails encryptionDetails { get; set; }
    }

    public class SPEncryptionDetails
    {
        //The encryption standard required to encrypt or decrypt the document contents.
        public string standard { get; set; }

        //The vector to encrypt or decrypt the document contents using Cipher Block Chaining (CBC).
        public string initializationVector { get; set; }

        //The encryption key used to encrypt or decrypt the document contents.
        public string key { get; set; }
    }
}
