using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RingCentral.Http
{
    public class Request : HttpHeaders
    {
        private readonly List<Attachment> _attachments;
        private readonly Dictionary<string, string> _formBody;
        private readonly string _jsonBody;
        private readonly List<KeyValuePair<string, string>> _queryValues;
        private readonly string _requestType;
        private readonly string _url;
        public bool HttpMethodTunneling { get; set; } = false;

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
        public string Url
        {
            get
            {
                return _url + Querystring;
            }
        }

        public Uri Uri
        {
            get
            {
                return new Uri(_url + Querystring, UriKind.Relative);
            }
        }

        /// <summary>
        ///     Returns HTTP content based on Content Type
        /// </summary>
        /// <returns>HttpContent based on Content Type</returns>
        public HttpContent HttpContent
        {
            get
            {
                if (string.IsNullOrEmpty(_requestType))
                {
                    return null;
                }

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
                        var fileContent = new ByteArrayContent(attachment.ByteArray);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = attachment.FileName
                        };
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                        multiPartContent.Add(fileContent);
                    }
                    return multiPartContent;
                }

                return null;
            }
        }

        /// <summary>
        ///     Gets the query string if query parameters are specified
        /// </summary>
        /// <returns>A query string that will be appended to the URL</returns>
        public string Querystring
        {
            get
            {
                if (_queryValues == null || !_queryValues.Any())
                {
                    return "";
                }

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

    public static class RequestUtil
    {
        public static void ApplyHttpMethodTunneling(this HttpRequestMessage requestMessage)
        {
            if (requestMessage.Method == HttpMethod.Put)
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Headers.Add("X-HTTP-Method-Override", "PUT");
            }
            else if (requestMessage.Method == HttpMethod.Delete)
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Headers.Add("X-HTTP-Method-Override", "DELETE");
            }
        }
    }

}