namespace RingCentral.SDK.Helper
{
    public class Attachment
    {

        private string FileName { get; set; }
        private string ContentType { get; set; }
        private byte[] ByteArrayContents { get; set; }

        /// <summary>
        /// Constructor that initializes an attachment that can be sent via fax
        /// </summary>
        /// <param name="fileName">Name of the attachment</param>
        /// <param name="contentType">content type that is a valid MIME type supported by Ring Central</param>
        /// <param name="byteArrayContents">the byte array contents of the file being attached</param>
        public Attachment(string fileName, string contentType, byte[] byteArrayContents)
        {
            FileName = fileName;
            ContentType = contentType;
            ByteArrayContents = byteArrayContents;
        }

        /// <summary>
        /// Gets the file name with a default of file.txt
        /// </summary>
        /// <returns>The file name</returns>
        public string GetFileName()
        {
            return !string.IsNullOrEmpty(FileName) ? FileName : "file.txt";
        }

        /// <summary>
        /// Gets the content type with a default of text/plain
        /// </summary>
        /// <returns>The Content Type</returns>
        public string GetContentType()
        {
            return !string.IsNullOrEmpty(ContentType) ? ContentType : "text/plain";
        }

        /// <summary>
        /// Gets the byte array contents
        /// </summary>
        /// <returns>The byte array contents</returns>
        public byte[] GetByteArrayContent()
        {
            return ByteArrayContents;
        }
    }
}
