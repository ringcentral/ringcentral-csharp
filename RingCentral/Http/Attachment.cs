namespace RingCentral.Http
{
    public class Attachment
    {
        private string fileName;
        private string contentType;
        public byte[] ByteArray { get; private set; }

        /// <summary>
        /// Constructor that initializes an attachment that can be sent via fax
        /// </summary>
        /// <param name="fileName">Name of the attachment</param>
        /// <param name="contentType">content type that is a valid MIME type supported by Ring Central</param>
        /// <param name="byteArray">the byte array contents of the file being attached</param>
        public Attachment(string fileName, string contentType, byte[] byteArray)
        {
            this.fileName = fileName;
            this.contentType = contentType;
            ByteArray = byteArray;
        }

        public string FileName
        {
            get
            {
                return !string.IsNullOrEmpty(fileName) ? fileName : "file.txt";
            }
        }

        public string ContentType
        {
            get
            {
                return !string.IsNullOrEmpty(contentType) ? contentType : "text/plain";
            }
        }
    }
}
