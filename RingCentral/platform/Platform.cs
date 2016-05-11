using RingCentral.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace RingCentral
{
    public class Platform
    {
        private const string AccessTokenTtl = "3600"; // 60 minutes
        private const string RefreshTokenTtl = "36000"; // 10 hours
        private const string RefreshTokenTtlRemember = "604800"; // 1 week
        private const string TokenEndpoint = "restapi/oauth/token";
        private const string RevokeEndpoint = "restapi/oauth/revoke";

        public HttpClient _client { private get; set; }
        public Auth Auth { get; private set; }

        private object thisLock = new object();

        public Platform(string appKey, string appSecret, string serverUrl, string appName = "", string appVersion = "")
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverUrl = serverUrl;
            Auth = new Auth();
            _client = new HttpClient { BaseAddress = new Uri(this.serverUrl) };
            SetUserAgentHeader(appName, appVersion);
        }

        private string appKey;
        private string appSecret;
        private string serverUrl;

        /// <summary>
        ///     Method to generate Access Token and Refresh Token to establish an authenticated session
        /// </summary>
        /// <param name="userName">Login of RingCentral user</param>
        /// <param name="password">Password of the RingCentral User</param>
        /// <param name="extension">Optional: Extension number to login</param>
        /// <param name="isRemember">If set to true, refresh token TTL will be one week, otherwise it's 10 hours</param>
        /// <returns>string response of Authenticate result.</returns>
        public ApiResponse Authorize(string userName, string extension, string password, bool isRemember)
        {
            var body = new Dictionary<string, string>
                       {
                           {"username", userName},
                           {"password", password},
                           {"extension", extension},
                           {"grant_type", "password"},
                           {"access_token_ttl", AccessTokenTtl},
                           {"refresh_token_ttl", isRemember ? RefreshTokenTtlRemember : RefreshTokenTtl}
                       };

            var request = new Request(TokenEndpoint, body);
            var result = AuthCall(request);

            Auth.SetRemember(isRemember);
            Auth.SetData(result.GetJson());

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            return result;
        }

        /// <summary>
        ///     Refreshes expired Access token during valid lifetime of Refresh Token
        /// </summary>
        /// <returns>string response of Refresh result</returns>
        public ApiResponse Refresh()
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
            var result = AuthCall(request);

            Auth.SetData(result.GetJson());

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            return result;
        }

        /// <summary>
        ///     Revokes the already granted access to stop application activity
        /// </summary>
        /// <returns>string response of Revoke result</returns>
        public ApiResponse Logout()
        {
            var body = new Dictionary<string, string>
                       {
                           {"token", Auth.GetAccessToken()}
                       };

            Auth.Reset();

            var request = new Request(RevokeEndpoint, body);

            return AuthCall(request);
        }

        /// <summary>
        ///     Authentication, Refresh and Revoke requests all require an Authentication Header Value of "Basic".  This is a
        ///     special method to handle those requests.
        /// </summary>
        /// <param name="request">
        ///     A Request object with a url and a dictionary of key value pairs (<c>Authenticate</c>,
        ///     <c>Refresh</c>, <c>Revoke</c>)
        /// </param>
        /// <returns>Response object</returns>
        private ApiResponse AuthCall(Request request)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GenerateAuthToken());

            var response = _client.PostAsync(request.GetUrl(), request.GetHttpContent()).Result;

            return new ApiResponse(response);
        }

        public ApiResponse Get(Request request)
        {
            return Send(HttpMethod.Get, request);
        }

        public ApiResponse Post(Request request)
        {
            return Send(HttpMethod.Post, request);
        }

        public ApiResponse Delete(Request request)
        {
            return Send(HttpMethod.Delete, request);
        }

        public ApiResponse Put(Request request)
        {
            return Send(HttpMethod.Put, request);
        }

        public ApiResponse Send(HttpMethod httpMethod, Request request)
        {
            if (!LoggedIn())
            {
                throw new Exception("Access has Expired");
            }

            var requestMessage = new HttpRequestMessage();
            requestMessage.Content = request.GetHttpContent();
            requestMessage.Method = httpMethod;
            requestMessage.RequestUri = request.GetUri();

            request.GetXhttpOverRideHeader(requestMessage);

            return new ApiResponse(_client.SendAsync(requestMessage).Result);
        }


        /// <summary>
        /// Generates auth token by encoding appKey and appSecret then converting it to base64
        /// </summary>
        /// <returns>The Api Key</returns>
        private string GenerateAuthToken()
        {
            var byteArray = Encoding.UTF8.GetBytes(appKey + ":" + appSecret);
            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        ///     You also may supply custom AppName:AppVersion in the form of a header with your application codename and version. These parameters
        ///     are optional but they will help a lot to identify your application in API logs and speed up any potential troubleshooting.
        ///     Allowed characters for AppName:AppVersion are- letters, digits, hyphen, dot and underscore.
        /// </summary>
        /// <param name="appName">Application Name</param>
        /// <param name="appVersion">Application Version</param>
        private void SetUserAgentHeader(string appName, string appVersion)
        {
            var agentString = String.Empty;

            #region Set UA String
            if (!string.IsNullOrEmpty(appName))
            {
                agentString += appName;
                if (!string.IsNullOrEmpty(appVersion))
                {
                    agentString += "_" + appVersion;
                }
            }
            if (string.IsNullOrEmpty(agentString))
            {
                agentString += "RCCSSDK_" + SDK.Version;
            }
            else
            {
                agentString += ".RCCSSDK_" + SDK.Version;
            }
            #endregion

            Regex r = new Regex("(?:[^a-z0-9-_. ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            var ua = r.Replace(agentString, String.Empty);

            _client.DefaultRequestHeaders.Add("User-Agent", ua);
            _client.DefaultRequestHeaders.Add("RC-User-Agent", ua);
        }

        /// <summary>
        ///     Determines if Access is valid and returns the boolean result.  If access is not valid but refresh token is valid
        ///     then a refresh is issued.
        /// </summary>
        /// <returns>boolean value of access authorization</returns>
        public bool LoggedIn()
        {
            if (Auth.IsAccessTokenValid())
            {
                return true;
            }

            if (Auth.IsRefreshTokenValid())
            {
                //obtain a mutual-exclusion lock for the thisLock object, execute statement and then release the lock.
                lock (thisLock)
                {
                    Refresh();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// When your application needs to authentiate an user, redirect the user to RingCentral API server for authentication.
        /// This method helps you to build the URI. Later you can redirect user to this URI.
        /// </summary>
        /// <param name="redirectUri">This is a callback URI which determines where the response will be sent to. The value of this parameter must exactly match one of the URIs you have provided for your app upon registration. This URI can be HTTP/HTTPS address for web applications or custom scheme URI for mobile or desktop applications.</param>
        /// <param name="state">Optional, recommended. An opaque value used by the client to maintain state between the request and callback. The authorization server includes this value when redirecting the user-agent back to the client. The parameter should be used for preventing cross-site request forgery</param>
        /// <returns></returns>
        public string AuthorizeUri(string redirectUri, string state = "")
        {
            var baseUrl = serverUrl + "/restapi/oauth/authorize";
            var authUrl = string.Format("{0}?response_type=code&state={1}&redirect_uri={2}&client_id={3}",
                baseUrl, Uri.EscapeUriString(state),
                Uri.EscapeUriString(redirectUri),
                Uri.EscapeUriString(appKey));
            return authUrl;
        }

        /// <summary>
        /// Do authentication with the authorization code returned from server
        /// </summary>
        /// <param name="authCode">The authorization code returned from server</param>
        /// <param name="redirectUri">The same redirectUri when you were obtaining the authCode in previous step</param>
        /// <returns></returns>
        public ApiResponse Authenticate(string authCode, string redirectUri)
        {
            var request = new Request("/restapi/oauth/token",
                new Dictionary<string, string> { { "grant_type", "authorization_code" },
                    { "redirect_uri", redirectUri }, { "code", authCode } });
            var response = AuthCall(request);
            Auth.SetData(response.GetJson());
            return response;
        }
    }
}
