using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingCentral.Http
{
    public class Headers
    {
        const string HeaderSeperator = ":";
        public const string ContentType = "content-type";
        const string Authorization = "authorization";
        public const string Accept = "accept";
        const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string JsonContentType = "application/json";
        const string MultipartContentType = "multipart/mixed";

        private readonly Dictionary<string, string> _headers;

        public Headers()
        {
            _headers = new Dictionary<string, string>();
        }
        
        public bool HasHeader(string name)
        {
            return _headers.ContainsKey(name);
        }

        public string GetHeader(string name)
        {
            return _headers.ContainsKey(name) ? _headers[name] : null;
        }

        public void SetHeader(string name, string value)
        {
            _headers.Add(name, value);
        }

        public void SetHeaders(Dictionary<string, string> headers)
        {
            foreach (var k in headers.Keys)
            {
                SetHeader(k, headers[k]);
            }
        }

        public Dictionary<string, string> GetHeaders()
        {
            return _headers;
        }

        public string[] GetHeadersArray()
        {
            return _headers.Keys.Select(k => k.ToLower() + HeaderSeperator + _headers[k]).ToArray();
        }

        public string GetContentType()
        {
            return GetHeader(ContentType);
        }

        public void SetContentType(string contentType)
        {
            SetHeader(ContentType, contentType);
        }

        public bool IsContentType(string contentType)
        {
            return GetContentType().Equals(contentType);
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
