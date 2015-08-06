using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace RingCentral.Http
{
    public class Headers
    {
        public const string ContentType = "content-type";
        public const string Accept = "accept";
        public const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string JsonContentType = "application/json";
        public const string MultipartContentType = "multipart/mixed";

        private HttpContentHeaders _headers;

        public void SetHeaders(HttpContentHeaders headers)
        {
            _headers = headers;
        }

        public HttpContentHeaders GetHeaders()
        {
            return _headers;
        }

        public string GetContentType()
        {
            return _headers.ContentType.ToString();
        }

        public bool IsContentType(string contentType)
        {
            return GetContentType().Contains(contentType);
        }

        public bool IsJson()
        {
            return IsContentType(JsonContentType);
        }

        public bool IsMultiPart()
        {
            return IsContentType(MultipartContentType);
        }

        public bool IsUrlEncoded()
        {
            return IsContentType(UrlEncodedContentType);
        }

    }
}
