using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace ringcentral.http
{
    class Request : Headers
    {
        private const string Post = "POST";
        private const string Get = "GET";
        private const string Delete = "DELETE";
        private const string Put = "PUT";
        private const string Patch = "PATCH";

        protected static List<String> AllowedMethods = new List<String>(new [] {Get,Post,Put,Delete});

        protected String Method;
        protected String Url;
        protected List<String> Query;
        protected List<String> Body;

        public Request(String method, String url, List<String> query, List<String> body)
        {
            Method = method;
            Url = url;
            Query = query;
            Body = body;

            var headers = new Dictionary<string, string> {{Accept, JsonContentType}, {ContentType, JsonContentType}};
            SetHeaders(headers);
        }

        public Boolean IsPost()
        {
            return Method.Equals(Post);
        }

        public Boolean IsGet()
        {
            return Method.Equals(Get);
        }

        public Boolean IsDelete()
        {
            return Method.Equals(Delete);
        }

        public Boolean IsPut()
        {
            return Method.Equals(Put);
        }

        public Boolean IsLoaded()
        {
            return false;
        }

        public String GetMethod()
        {
            return Method;
        }

        public void SetMethod(String method)
        {
            if (!AllowedMethods.Contains(method))
            {
                throw new Exception("Unknown method");
            }

            Method = method;
        }

        public String GetUrl()
        {
            return Url;
        }

        public void SetUrl(String url)
        {
            Url = url;
        }

        public List<String> GetQuery()
        {
            return Query;
        }

        public void SetQuery(List<String> query)
        {
            Query = query;
        }

        public List<String> GetBody()
        {
            return Body;
        }

        public void SetBody(List<String> body)
        {
            Body = body;
        }

        public void Send()
        {
          
        }

        private String GetEncodedBody()
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

        private String GetUrlWithQuery()
        {
            var url = Url;
            //TODO: Determine if Query is the right datatype and then properly encode
            var query = Uri.EscapeUriString(Query.ToString());

            return query;

        }

    }
}
