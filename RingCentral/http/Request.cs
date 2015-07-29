using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace RingCentral.Http
{
    public class Request : Headers
    {
        private const string Post = "POST";
        private const string Get = "GET";
        private const string Delete = "DELETE";
        private const string Put = "PUT";
        private const string Patch = "PATCH";

        protected static List<string> AllowedMethods = new List<string>(new[] {Get, Post, Put, Delete});

        protected string Method;
        protected string Url;

        public Request(string method, string url, List<KeyValuePair<string, string>> query,
            Dictionary<String, String> body, HttpContentHeaders headers)
        {
            Method = method;
            Url = url;
            Query = query;
            Body = body;

            var contentHeaders = new Dictionary<string, string>
                                 {
                                     {Accept, JsonContentType},
                                     {ContentType, JsonContentType}
                                 };
            SetHeaders(contentHeaders);

            if (headers != null && headers.Any())
            {
                SetHeaders(headers);
            }
        }

        private List<KeyValuePair<string, string>> Query { get; set; }
        private Dictionary<string, string> Body { get; set; }

        public bool IsPost()
        {
            return Method.Equals(Post);
        }

        public bool IsGet()
        {
            return Method.Equals(Get);
        }

        public bool IsDelete()
        {
            return Method.Equals(Delete);
        }

        public bool IsPut()
        {
            return Method.Equals(Put);
        }

        public bool IsLoaded()
        {
            return false;
        }

        public string GetMethod()
        {
            return Method;
        }

        public void SetMethod(string method)
        {
            if (!AllowedMethods.Contains(method))
            {
                throw new Exception("Unknown method");
            }

            Method = method;
        }

        public string GetUrl()
        {
            return Url;
        }

        public void SetUrl(string url)
        {
            Url = url;
        }

        public List<KeyValuePair<string, string>> GetQuery()
        {
            return Query;
        }

        public void SetQuery(List<KeyValuePair<string, string>> query)
        {
            Query = query;
        }

        public Dictionary<string, string> GetBody()
        {
            return Body;
        }

        public void SetBody(Dictionary<string, string> body)
        {
            Body = body;
        }

        public void Send()
        {
        }

        private string GetEncodedBody()
        {
            if (IsJson())
            {
                return JsonConvert.SerializeObject(Body);
            }
            //TODO: Determine if Body is the right data type and then properly encode.  Portable .net 4 doesn't have access to HTTPUtility or WebUtility it seems.
            if (IsUrlEncoded())
            {
                return Uri.EscapeDataString(Body.ToString()).Replace('+', ' ');
            }

            return "";
        }

        private string GetUrlWithQuery()
        {
            string url = Url;
            //TODO: Determine if Query is the right datatype and then properly encode
            string query = Uri.EscapeUriString(Query.ToString());

            return query;
        }
    }
}