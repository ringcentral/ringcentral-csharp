using System.Net.Http.Headers;

namespace RingCentral.SDK.Http
{
    public class Headers
    {
        public const string ContentType = "content-type";
        public const string Accept = "accept";
        public const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string JsonContentType = "application/json";
        public const string MultipartContentType = "multipart/mixed";
        private HttpContentHeaders _headers;

        /// <summary>
        ///     Sets the HttpContentHeaders
        /// </summary>
        /// <param name="headers">HttpContentHeaders</param>
        public void SetHeaders(HttpContentHeaders headers)
        {
            _headers = headers;
        }

        /// <summary>
        ///     Gets the HttpContentHeaders
        /// </summary>
        /// <returns>HttpContentHeaders</returns>
        public HttpContentHeaders GetHeaders()
        {
            return _headers;
        }

        /// <summary>
        ///     Gets the ContentType in the headers
        /// </summary>
        /// <returns>ContentType in the HttpContentHeaders</returns>
        public string GetContentType()
        {
            return _headers.ContentType.ToString();
        }

        /// <summary>
        ///     Determines if the content is present in the headers
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns>bool value if content type is in headers</returns>
        public bool IsContentType(string contentType)
        {
            return GetContentType().Contains(contentType);
        }

        /// <summary>
        ///     Inspects headers to determine if content type is JSON
        /// </summary>
        /// <returns>bool value if headers contain JSON content type</returns>
        public bool IsJson()
        {
            return IsContentType(JsonContentType);
        }

        /// <summary>
        ///     Inspects headers to determine if content type is MultiPart
        /// </summary>
        /// <returns>bool value if headers contain MultiPart content type</returns>
        public bool IsMultiPart()
        {
            return IsContentType(MultipartContentType);
        }

        /// <summary>
        ///     Inspects headers to determine if content type is UrlEncoded
        /// </summary>
        /// <returns>bool value if headers containt UrlEncoded content type</returns>
        public bool IsUrlEncoded()
        {
            return IsContentType(UrlEncodedContentType);
        }
    }
}