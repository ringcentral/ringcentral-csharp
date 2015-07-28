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
        const string HeaderSeperator = ":";
        public const string ContentType = "content-type";
        const string Authorization = "authorization";
        public const string Accept = "accept";
        const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string JsonContentType = "application/json";
        const string MultipartContentType = "multipart/mixed";

        private HttpContentHeaders _headers;

        //public Headers(HttpContentHeaders headers)
        //{
        //    _headers = headers;
        //}
        
        public bool HasHeader(string name)
        {
            return _headers.Contains(name);
        }


        public void SetHeader(string name, string value)
        {
            _headers.Add(name, value);
        }

        public void SetHeaders(Dictionary<string, string> headers)
        {
            foreach (var k in headers.Where(k => !_headers.Contains(k.Key)))
            {
                SetHeader(k.Key, k.Value);
            }
        }

        public void SetHeaders(HttpContentHeaders headers)
        {
            _headers = headers;
        }

        public HttpContentHeaders GetHeaders()
        {
            return _headers;
        }

        public string[] GetHeadersArray()
        {
            return _headers.Select(header => header.Key.ToLower() + HeaderSeperator + header.Value).ToArray();
        }

        public string GetContentType()
        {
            return _headers.ContentType.ToString();
        }

        public void SetContentType(string contentType)
        {
            SetHeader(ContentType, contentType);
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
