using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ringcentral.http
{
    class Headers
    {
        const string HeaderSeperator = ":";
        public const string ContentType = "content-type";
        const string Authorization = "authorization";
        public const string Accept = "accept";
        const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string JsonContentType = "application/json";
        const string MultipartContentType = "multipart/mixed";

        private readonly Dictionary<String, String> _headers;

        public Headers()
        {
            _headers = new Dictionary<String, String>();
        }
        
        public Boolean HasHeader(String name)
        {
            return _headers.ContainsKey(name);
        }

        public String GetHeader(String name)
        {
            return _headers.ContainsKey(name) ? _headers[name] : null;
        }

        public void SetHeader(String name, String value)
        {
            _headers.Add(name, value);
        }

        public void SetHeaders(Dictionary<String, String> headers)
        {
            foreach (var k in headers.Keys)
            {
                SetHeader(k, headers[k]);
            }
        }

        public Dictionary<String, String> GetHeaders()
        {
            return _headers;
        }

        public String[] GetHeadersArray()
        {
            return _headers.Keys.Select(k => k.ToLower() + HeaderSeperator + _headers[k]).ToArray();
        }

        public String GetContentType()
        {
            return GetHeader(ContentType);
        }

        public void SetContentType(String contentType)
        {
            SetHeader(ContentType, contentType);
        }

        public Boolean IsContentType(String contentType)
        {
            return GetContentType().Equals(contentType);
        }

        public Boolean IsJson()
        {
            return IsContentType(JsonContentType);
        }

        public Boolean IsMultiPart()
        {
            return IsContentType(MultipartContentType);
        }

        public Boolean IsUrlEncoded()
        {
            return IsContentType(UrlEncodedContentType);
        }

    }
}
