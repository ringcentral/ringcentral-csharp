using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace RingCentral.Helper
{
    public class Attachment
    {

        private string FileName { get; set; }
        private string ContentType { get; set; }
        private byte[] ByteArrayContents { get; set; }

        public Attachment(string fileName, string contentType, byte[] byteArrayContents)
        {
            FileName = fileName;
            ContentType = contentType;
            ByteArrayContents = byteArrayContents;
        }

        public string GetFileName()
        {
            return !string.IsNullOrEmpty(FileName) ? FileName : "file.txt";
        }

        public string GetContentType()
        {
            return !string.IsNullOrEmpty(ContentType) ? ContentType : "text/plain";
        }

        public byte[] GetByteArrayContent()
        {
            return ByteArrayContents;
        }
    }
}
