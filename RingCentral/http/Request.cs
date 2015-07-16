using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace RingCentral.Http
{
    class Request : Headers
    {
        private const string Post = "POST";
        private const string Get = "GET";
        private const string Delete = "DELETE";
        private const string Put = "PUT";
        private const string Patch = "PATCH";

        protected static List<string> AllowedMethods = new List<string>(new [] {Get,Post,Put,Delete});

        protected string Method;
        protected string Url;
        protected List<string> Query;
        protected List<string> Body;

        public Request(string method, string url, List<string> query, List<string> body)
        {
            Method = method;
            Url = url;
            Query = query;
            Body = body;

            var headers = new Dictionary<string, string> {{Accept, JsonContentType}, {ContentType, JsonContentType}};
            SetHeaders(headers);
        }

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

        public List<string> GetQuery()
        {
            return Query;
        }

        public void SetQuery(List<string> query)
        {
            Query = query;
        }

        public List<string> GetBody()
        {
            return Body;
        }

        public void SetBody(List<string> body)
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
            var url = Url;
            //TODO: Determine if Query is the right datatype and then properly encode
            var query = Uri.EscapeUriString(Query.ToString());

            return query;

        }

    }
}
