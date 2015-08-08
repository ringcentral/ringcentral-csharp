using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RingCentral.Http;

namespace RingCentral
{
    public class Platform
    {
        private const string AccessTokenTtl = "3600"; // 60 minutes
        private const string RefreshTokenTtl = "36000"; // 10 hours
        private const string RefreshTokenTtlRemember = "604800"; // 1 week
        private const string TokenEndpoint = "/restapi/oauth/token";
        private const string RevokeEndpoint = "/restapi/oauth/revoke";
        private HttpClient _client;
        protected Auth Auth;

        public Platform(string appKey, string appSecret, string apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
            Auth = new Auth();
            _client = new HttpClient {BaseAddress = new Uri(ApiEndpoint)};
            _client.DefaultRequestHeaders.Add("SDK-Agent", "Ring Central C# SDK");
        }

        private string AppKey { get; set; }
        private string AppSecret { get; set; }
        private string ApiEndpoint { get; set; }

        /// <summary>
        ///     Method to generate Access Token and Refresh Token to establish an authenticated session
        /// </summary>
        /// <param name="userName">Login of RingCentral user</param>
        /// <param name="password">Password of the RingCentral User</param>
        /// <param name="extension">Optional: Extension number to login</param>
        /// <param name="isRemember">If set to true, refresh token TTL will be one week, otherwise it's 10 hours</param>
        /// <returns>string response of Authenticate result.</returns>
        public string Authenticate(string userName, string password, string extension, bool isRemember)
        {
            var body = new Dictionary<string, string>
                       {
                           {"username", userName},
                           {"password", Uri.EscapeUriString(password)},
                           {"extension", extension},
                           {"grant_type", "password"},
                           {"access_token_ttl", AccessTokenTtl},
                           {"refresh_token_ttl", isRemember ? RefreshTokenTtlRemember : RefreshTokenTtl}
                       };

            var request = new Request(TokenEndpoint, body);
            var result = AuthPostRequest(request);

            Auth.SetRemember(isRemember);
            Auth.SetData(JObject.Parse(result));

            return result;
        }

        /// <summary>
        ///     Refreshes expired Access token during valid lifetime of Refresh Token
        /// </summary>
        /// <returns>string response of Refresh result</returns>
        public string Refresh()
        {
            if (!Auth.IsRefreshTokenValid()) throw new Exception("Refresh Token has Expired");

            var body = new Dictionary<string, string>
                       {
                           {"grant_type", "refresh_token"},
                           {"refresh_token", Auth.GetRefreshToken()},
                           {"access_token_ttl", AccessTokenTtl},
                           {"refresh_token_ttl", Auth.IsRemember() ? RefreshTokenTtlRemember : RefreshTokenTtl}
                       };

            var request = new Request(TokenEndpoint, body);
            var result = AuthPostRequest(request);

            Auth.SetData(JObject.Parse(result));

            return result;
        }

        /// <summary>
        ///     Revokes the already granted access to stop application activity
        /// </summary>
        /// <returns>string response of Revoke result</returns>
        public string Revoke()
        {
            var body = new Dictionary<string, string>
                       {
                           {"token", Auth.GetAccessToken()}
                       };

            Auth.Reset();

            var request = new Request(RevokeEndpoint, body);

            return AuthPostRequest(request);
        }

        /// <summary>
        ///     Authentication, Refresh and Revoke requests all require an Authentication Header Value of "Basic".  This is a
        ///     special
        ///     method to handle those requests.
        /// </summary>
        /// <param name="endPoint">
        ///     This endpoint will be the value passed in depending on the request issued (<c>Authenticate</c>,
        ///     <c>Refresh</c>, <c>Revoke</c>)
        /// </param>
        /// <returns>string response of the AuthPostRequest</returns>
        public string AuthPostRequest(Request request)
        {
            SetXhttpOverRideHeader(request.GetXhttpOverRideHeader());

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetApiKey());

            var result = _client.PostAsync(request.GetUrl(), request.GetHttpContent()).Result;

            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        ///     A HTTP POST request.  If StringBody is set via <c>SetStringData</c> it will set the content type of
        ///     application/json.
        ///     If form paramaters are set via <c>AddFormParameter</c> then it will post those values
        /// </summary>
        /// <param name="endPoint">The Endpoint of the POST request targeted</param>
        /// <returns>The string value of the POST request result</returns>
        public Response PostRequest(Request request)
        {
            CheckAccessAndOverRideHeaders(request);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            var postResult = _client.PostAsync(request.GetUrl(), request.GetHttpContent());

            return SetResponse(postResult);
        }

        /// <summary>
        ///     A HTTP GET request.  If query parameters are set via <c>AddQueryParameters</c> then they will be included in the
        ///     GET request
        /// </summary>
        /// <param name="endPoint">The Endpoint of the GET request</param>
        /// <returns>string response of the GET request</returns>
        public Response GetRequest(Request request)
        {
            CheckAccessAndOverRideHeaders(request);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            var result = _client.GetAsync(request.GetUrl());

            return SetResponse(result);
        }

        /// <summary>
        ///     A HTTP DELETE request.
        /// </summary>
        /// <param name="endPoint">The Endpoint of the DELETE request</param>
        /// <returns>string response of the DELETE request</returns>
        public Response DeleteRequest(Request request)
        {
            if (!IsAccessValid()) throw new Exception("Access has Expired");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            var deleteResult = _client.DeleteAsync(request.GetUrl());

            return SetResponse(deleteResult);
        }

        /// <summary>
        ///     A HTTP PUT request.  If StringBody is set via <c>SetStringData</c> it will set the content type of
        ///     application/json.
        ///     If form paramaters are set via <c>AddFormParameter</c> then it will post those values
        /// </summary>
        /// <param name="endPoint">The Endpoint of the PUT request</param>
        /// <returns>string response of the PUT request</returns>
        public Response PutRequest(Request request)
        {
            if (!IsAccessValid()) throw new Exception("Access has Expired");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            var putResult = _client.PutAsync(request.GetUrl(), request.GetHttpContent());

            return SetResponse(putResult);
        }

        private static Response SetResponse(Task<HttpResponseMessage> responseMessage)
        {
            var statusCode = Convert.ToInt32(responseMessage.Result.StatusCode);
            var body = responseMessage.Result.Content.ReadAsStringAsync().Result;
            var headers = responseMessage.Result.Content.Headers;

            return new Response(statusCode, body, headers);
        }

        private string GetApiKey()
        {
            var byteArray = Encoding.UTF8.GetBytes(AppKey + ":" + AppSecret);
            return Convert.ToBase64String(byteArray);
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        public void SetClient(HttpClient client)
        {
            _client = client;
        }

        private void CheckAccessAndOverRideHeaders(Request request)
        {
            if (!IsAccessValid()) throw new Exception("Access has Expired");

            SetXhttpOverRideHeader(request.GetXhttpOverRideHeader());
        }

        private void SetXhttpOverRideHeader(string method)
        {
            if (!string.IsNullOrEmpty(method))
            {
                _client.DefaultRequestHeaders.Add("X-HTTP-Method-Override", method.ToUpper()); 
            }
            if (method == null && _client.DefaultRequestHeaders.Contains("X-HTTP-Method-Override"))
            {
                _client.DefaultRequestHeaders.Remove("X-HTTP-Method-Override");
            }
        }

        public void SetUserAgentHeader(string header)
        {
            _client.DefaultRequestHeaders.Add("User-Agent", header);
        }

        public bool IsAccessValid()
        {
            if (Auth.IsAccessTokenValid())
            {
                return true;
            }

            if (Auth.IsRefreshTokenValid())
            {
                Refresh();
                return true;
            }
            return false;
        }
    }
}