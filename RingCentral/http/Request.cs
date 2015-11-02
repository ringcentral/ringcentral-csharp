using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using RingCentral.SDK.Helper;

namespace RingCentral.SDK.Http
{
    public class Request : Headers
    {
        protected static List<string> RequestTypes =
            new List<string>(new[] {UrlEncodedContentType, JsonContentType, MultipartContentType});

        private readonly List<Attachment> _attachments;
        private readonly Dictionary<string, string> _formBody;
        private readonly string _jsonBody;
        private readonly List<KeyValuePair<string, string>> _queryValues;
        private readonly string _requestType;
        private readonly string _url;
        private string _xHttpOverrideHeader;

        /// <summary>
        ///     Creates a request object with a URL endpoint specified
        /// </summary>
        /// <param name="url">The URL endpoint</param>
        public Request(string url)
        {
            _url = url;
        }

        /// <summary>
        ///     Creates a request object with URL endpoint specified as well as query parameters
        /// </summary>
        /// <param name="url">The URL endpoint</param>
        /// <param name="queryValues">Query parameters that will be appended to the URL</param>
        public Request(string url, List<KeyValuePair<string, string>> queryValues)
        {
            _url = url;
            _queryValues = queryValues;
        }

        /// <summary>
        ///     Creates a request object with JSON string body
        /// </summary>
        /// <param name="url">THe URL endpoint</param>
        /// <param name="jsonBody">The fully formed JSON body</param>
        public Request(string url, string jsonBody)
        {
            _url = url;
            _jsonBody = jsonBody;
            _requestType = JsonContentType;
        }

        /// <summary>
        ///     Creates a request object with form parameters
        /// </summary>
        /// <param name="url">The URL endpoint</param>
        /// <param name="formBody">Form parameters</param>
        public Request(string url, Dictionary<string, string> formBody)
        {
            _url = url;
            _formBody = formBody;
            _requestType = UrlEncodedContentType;
        }

        /// <summary>
        ///     Creates a request object that can is used for MultiPartContent
        /// </summary>
        /// <param name="url">The endpoint URL</param>
        /// <param name="jsonBody">The fully formed JSON body</param>
        /// <param name="attachments">A list of Attachment objects</param>
        public Request(string url, string jsonBody, List<Attachment> attachments)
        {
            _url = url;
            _jsonBody = jsonBody;
            _requestType = MultipartContentType;
            _attachments = attachments;
        }

        /// <summary>
        ///     Gets the URL in addition to the QueryString if it is populated
        /// </summary>
        /// <returns>A URL with Query String appended if query values are present</returns>
        public string GetUrl()
        {
            return _url + GetQuerystring();
        }

        public Uri GetUri()
        {
            return new Uri(_url + GetQuerystring(),UriKind.Relative);
        }

        public HttpMethod GetHttpMethod(string method)
        {
            if (method.Equals("GET"))
            {
                return HttpMethod.Get;
            }

            if (method.Equals("POST"))
            {
                return HttpMethod.Post;
            }

            if (method.Equals("PUT"))
            {
                return HttpMethod.Put;
            }

            if (method.Equals("DELETE"))
            {
                return HttpMethod.Delete;
            }

            return null;
        }

        /// <summary>
        ///     Returns HTTP content based on Content Type
        /// </summary>
        /// <returns>HttpContent based on Content Type</returns>
        public HttpContent GetHttpContent()
        {
            if (string.IsNullOrEmpty(_requestType)) return null;

            if (_requestType.Equals(JsonContentType))
            {
                return new StringContent(_jsonBody, Encoding.UTF8, "application/json");
            }

            if (_requestType.Equals(UrlEncodedContentType))
            {
                var formBodyList = _formBody.ToList();

                return new FormUrlEncodedContent(formBodyList);
            }
            if (_requestType.Equals(MultipartContentType))
            {
                var multiPartContent = new MultipartFormDataContent("Boundary_1_14413901_1361871080888");

                //removes content type of multipart form data
                multiPartContent.Headers.Remove("Content-Type");

                //need to set content type to multipart/mixed
                multiPartContent.Headers.TryAddWithoutValidation("Content-Type",
                    "multipart/mixed; charset=UTF-8; boundary=Boundary_1_14413901_1361871080888");
                multiPartContent.Add(new StringContent(_jsonBody, Encoding.UTF8, "application/json"));


                foreach (var attachment in _attachments)
                {
                    var fileContent = new ByteArrayContent(attachment.GetByteArrayContent());
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                                             {
                                                                 FileName = attachment.GetFileName()
                                                             };
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.GetContentType());
                    multiPartContent.Add(fileContent);
                }

                return multiPartContent;
            }


            return null;
        }

        /// <summary>
        ///     Gets the query string if query parameters are specified
        /// </summary>
        /// <returns>A query string that will be appended to the URL</returns>
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

        public void GetXhttpOverRideHeader(HttpRequestMessage requestMessage)
        {

            if (_xHttpOverrideHeader == "PUT")
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Headers.Add("X-HTTP-Method-Override", "PUT");
            }
            if (_xHttpOverrideHeader == "DELETE")
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Headers.Add("X-HTTP-Method-Override", "DELETE");
            }

        }

        /// <summary>
        ///     Sets the X-HTTP-Method-Override-Header
        /// </summary>
        /// <param name="method">The method that will be used to override</param>
        public void SetXhttpOverRideHeader(string method)
        {
            var allowedMethods = new List<string>(new[] {"GET", "POST", "PUT", "DELETE"});

            if (method != null && allowedMethods.Contains(method.ToUpper()))
            {
                _xHttpOverrideHeader = method;
            }
        }
    }
}