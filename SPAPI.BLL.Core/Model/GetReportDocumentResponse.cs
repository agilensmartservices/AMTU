namespace SPAPI.BLL.Core.Model
{
    public class GetReportDocumentResponse
    {
        //required - The identifier for the report document. This identifier is unique only in combination with a seller ID.
        public string reportDocumentId { get; set; }

        //required - A presigned URL for the report document. This URL expires after 5 minutes.
        public string url { get; set; }

        //If present, the report document contents have been compressed with the provided algorithm
        public CompressionAlgorithm compressionAlgorithm { get; set; }
    }

    public class ReportDocumentEncryptionDetails
    {
        //required - The encryption standard required to decrypt the document contents.
        public Standard standard { get; set; }

        //required - The vector to decrypt the document contents using Cipher Block Chaining (CBC).	
        public string initializationVector { get; set; }

        //required - The encryption key used to decrypt the document contents.
        public string key { get; set; }
    }

    //The encryption standard required to decrypt the document contents.
    public enum Standard
    {
        AES //The Advanced Encryption Standard (AES)
    }

    public enum CompressionAlgorithm
    {
        GZIP //The gzip compression algorithm.
    }
}
