using System.Net.Http.Headers;

namespace RingCentral.Http
{
    public class HttpHeaders
    {
        public const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string JsonContentType = "application/json";
        public const string MultipartContentType = "multipart/mixed";
        public HttpContentHeaders Headers { get; protected set; }

        /// <summary>
        ///     Gets the ContentType in the headers
        /// </summary>
        /// <returns>ContentType in the HttpContentHeaders</returns>
        public string GetContentType()
        {
            return Headers.ContentType.ToString();
        }

        /// <summary>
        ///     Determines if the content is present in the headers
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns>bool value if content type is in headers</returns>
        private bool IsContentType(string contentType)
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