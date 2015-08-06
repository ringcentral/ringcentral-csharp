using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RingCentral.Http
{
    public class Request : Headers
    {
        protected static List<string> RequestTypes =
            new List<string>(new[] {UrlEncodedContentType, JsonContentType, MultipartContentType});

        private readonly Dictionary<string, string> _formBody;
        private readonly string _jsonBody;
        private readonly List<KeyValuePair<string, string>> _queryValues;
        private readonly string _requestType;
        private readonly string _url;

        public Request(string url)
        {
            _url = url;
        }

        public Request(string url, List<KeyValuePair<string, string>> queryValues)
        {
            _url = url;
            _queryValues = queryValues;
        }

        public Request(string url, string jsonBody)
        {
            _url = url;
            _jsonBody = jsonBody;
            _requestType = JsonContentType;
        }

        public Request(string url, Dictionary<string, string> formBody)
        {
            _url = url;
            _formBody = formBody;
            _requestType = UrlEncodedContentType;
        }

        public string GetUrl()
        {
            return _url + GetQuerystring();
        }

        public List<KeyValuePair<string, string>> GetQueryValues()
        {
            return _queryValues;
        }

        public HttpContent GetHttpContent()
        {
            if (_requestType.Equals(JsonContentType))
            {
                return new StringContent(_jsonBody, Encoding.UTF8, "application/json");
            }

            if (_requestType.Equals(UrlEncodedContentType))
            {
                var formBodyList = _formBody.ToList();

                return new FormUrlEncodedContent(formBodyList);
            }
            //TODO: need to configure this to work dynamically with faxes
            if (_requestType.Equals(MultipartContentType))
            {
                var multiPartContent = new MultipartFormDataContent("Boundary_1_14413901_1361871080888");
                var json =
                    "{\"to\":[{\"phoneNumber\":\"***REMOVED***\"}],\"faxResolution\":\"High\",\"sendTime\":\"2013-02-26T09:31:20.882Z\"}";

                multiPartContent.Headers.Remove("Content-Type");
                multiPartContent.Headers.TryAddWithoutValidation("Content-Type",
                    "multipart/mixed; charset=UTF-8; boundary=Boundary_1_14413901_1361871080888");
                multiPartContent.Add(new StringContent(json, Encoding.UTF8, "application/json"));

                var stringContent = new StringContent("Hello World!!", Encoding.UTF8, "text/plain");
                stringContent.Headers.ContentType.CharSet = "";
                stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                                           {
                                                               FileName
                                                                   =
                                                                   "myfile.csv"
                                                           };

                multiPartContent.Add(stringContent);
                Debug.WriteLine("returning multipart content");

                return multiPartContent;
            }
            return null;
        }

        /// <summary>
        ///     Gets the query string after they were set by <c>AddQueryParameters</c>
        /// </summary>
        /// <returns>A query string</returns>
        public string GetQuerystring()
        {
            if (_queryValues == null || !_queryValues.Any()) return "";

            var querystring = "?";

            var last = _queryValues.Last();

            foreach (var parameter in _queryValues)
            {
                querystring = querystring + (parameter.Key + "=" + parameter.Value);
                if (!parameter.Equals(last))
                {
                    querystring += "&";
                }
            }

            return querystring;
        }
    }
}