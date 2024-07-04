using System;

namespace SPAPI.BLL.Core.Model
{
    public class CreateFeedDocumentResponse
    {
        public string feedDocumentId { get; set; }

        //The presigned URL for uploading the feed contents. This URL expires after 5 minutes.
        public string url { get; set; }
    }

    public class CreateFeedDocumentResult
    {
        //The identifier of the feed document.
        public string feedDocumentId { get; set; }

        //The presigned URL for uploading the feed contents. This URL expires after 5 minutes.
        public string url { get; set; }

        //Encryption details for required client-side encryption and decryption of document contents.
        public EncryptionDetails encryptionDetails { get; set; }
    }

    public class EncryptionDetails
    {
        //The encryption standard required to encrypt or decrypt the document contents.
        public string standard { get; set; }

        //The vector to encrypt or decrypt the document contents using Cipher Block Chaining (CBC).
        public string initializationVector { get; set; }

        //The encryption key used to encrypt or decrypt the document contents.
        public string key { get; set; }
    }
}
