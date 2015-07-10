using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
    }
}
