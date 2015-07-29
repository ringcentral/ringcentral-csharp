using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RingCentral.Http;

namespace RingCentral
{
    public class Platform
    {        
        protected Auth Auth;
        private HttpClient _client;

        public Platform(string appKey, string appSecret, string apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
            Auth = new Auth();
            _client = new HttpClient {BaseAddress = new Uri(ApiEndpoint)};
        }

        private string AppKey { get; set; }
        private string AppSecret { get; set; }
        private string ApiEndpoint { get; set; }

        private const string ACCESS_TOKEN_TTL = "3600"; // 60 minutes
        private const string REFRESH_TOKEN_TTL = "36000"; // 10 hours
        private const string REFRESH_TOKEN_TTL_REMEMBER = "604800"; // 1 week

        private List<KeyValuePair<string, string>> QueryParameters { get; set; }
        private Dictionary<string, string> FormParameters { get; set; }

        private string JsonData { get; set; }
        public bool IsMultiPartResponse { get; set; }

        /// <summary>
        ///     Method to generate Access Token and Refresh Token to establish an authenticated session
        /// </summary>
        /// <param name="userName">Login of RingCentral user</param>
        /// <param name="password">Password of the RingCentral User</param>
        /// <param name="extension">Optional: Extension number to login</param>
        /// <param name="endPoint">Authentication Endpoint</param>
        /// <returns>string response of Authenticate result.</returns>
        public string Authenticate(string userName, string password, string extension, string endPoint)
        {
            FormParameters = new Dictionary<string, string>
                             {
                                 {"username", userName},
                                 {"password", Uri.EscapeUriString(password)},
                                 {"extension", extension},
                                 {"grant_type", "password"},
                                 {"access_token_ttl", ACCESS_TOKEN_TTL},
                                 {"refresh_token_ttl", REFRESH_TOKEN_TTL}
                             };

            string result = AuthPostRequest(endPoint);

            Auth.SetData(JObject.Parse(result));

            return result;
        }

        /// <summary>
        ///     Refreshes expired Access token during valid lifetime of Refresh Token
        /// </summary>
        /// <param name="endPoint">The Refresh token endpoint</param>
        /// <returns>string response of Refresh result</returns>
        public string Refresh(string endPoint)
        {
            if (!Auth.IsRefreshTokenValid()) throw new Exception("Refresh Token has Expired");

            FormParameters = new Dictionary<string, string>
                             {
                                 {"grant_type", "refresh_token"},
                                 {"refresh_token", Auth.GetRefreshToken()},
                                 {"access_token_ttl", ACCESS_TOKEN_TTL},
                                 {"refresh_token_ttl", REFRESH_TOKEN_TTL}
                             };

            string result = AuthPostRequest(endPoint);

            Auth.SetData(JObject.Parse(result));

            return result;
        }

        /// <summary>
        ///     Revokes the already granted access to stop application activity
        /// </summary>
        /// <param name="endPoint">The Revoke Endpoint</param>
        /// <returns>string response of Revoke result</returns>
        public string Revoke(string endPoint)
        {
            FormParameters = new Dictionary<string, string>
                             {
                                 {"token", Auth.GetAccessToken()}
                             };

            Auth.Reset();

            return AuthPostRequest(endPoint);
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
        public string AuthPostRequest(string endPoint)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetApiKey());

            HttpResponseMessage result = _client.PostAsync(endPoint, GetFormParameters()).Result;

            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        ///     A HTTP POST request.  If JsonData is set via <c>SetJsonData</c> it will set the content type of application/json.
        ///     If form paramaters are set via <c>AddFormParameter</c> then it will post those values
        /// </summary>
        /// <param name="endPoint">The Endpoint of the POST request targeted</param>
        /// <returns>The string value of the POST request result</returns>
        public Response PostRequest(string endPoint)
        {
            if (!Auth.IsAccessTokenValid()) throw new Exception("Access has Expired");

            HttpContent httpContent = GetHttpContent(_client);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

           Task<HttpResponseMessage> postResult = _client.PostAsync(endPoint, httpContent);

           return SetResponse(postResult);
        }

        /// <summary>
        ///     A HTTP GET request.  If query parameters are set via <c>AddQueryParameters</c> then they will be included in the
        ///     GET request
        /// </summary>
        /// <param name="endPoint">The Endpoint of the GET request</param>
        /// <returns>string response of the GET request</returns>
        public Response GetRequest(string endPoint)
        {
            if (!Auth.IsAccessTokenValid()) throw new Exception("Access has Expired");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            endPoint += GetQuerystring();

            Task<HttpResponseMessage> result = _client.GetAsync(endPoint);

            ClearQueryParameters();

            return SetResponse(result);

        }

        /// <summary>
        ///     A HTTP DELETE request.
        /// </summary>
        /// <param name="endPoint">The Endpoint of the DELETE request</param>
        /// <returns>string response of the DELETE request</returns>
        public Response DeleteRequest(string endPoint)
        {
            if (!Auth.IsAccessTokenValid()) throw new Exception("Access has Expired");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            endPoint += GetQuerystring();

            Task<HttpResponseMessage> deleteResult = _client.DeleteAsync(endPoint);

            return SetResponse(deleteResult);
        }

        /// <summary>
        ///     A HTTP PUT request.  If JsonData is set via <c>SetJsonData</c> it will set the content type of application/json.
        ///     If form paramaters are set via <c>AddFormParameter</c> then it will post those values
        /// </summary>
        /// <param name="endPoint">The Endpoint of the PUT request</param>
        /// <returns>string response of the PUT request</returns>
        public Response PutRequest(string endPoint)
        {
            if (!Auth.IsAccessTokenValid()) throw new Exception("Access has Expired");

            HttpContent httpContent = GetHttpContent(_client);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.GetAccessToken());

            endPoint += GetQuerystring();

            Task<HttpResponseMessage> putResult = _client.PutAsync(endPoint, httpContent);

            return SetResponse(putResult);
        }

        public Response SetResponse(Task<HttpResponseMessage> responseMessage)
        {

            var statusCode = Convert.ToInt32(responseMessage.Result.StatusCode);
            var body = responseMessage.Result.Content.ReadAsStringAsync().Result;
            var headers = responseMessage.Result.Content.Headers;

            return new Response(statusCode,body,headers);
        }

        public HttpContent GetHttpContent(HttpClient client)
        {
            HttpContent httpContent;

            if (JsonData != null)
            {
                httpContent = new StringContent(JsonData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            }
            else
            {
                httpContent = GetFormParameters();
            }

            return httpContent;
        }

        /// <summary>
        ///     Gets the query string after they were set by <c>AddQueryParameters</c>
        /// </summary>
        /// <returns>A query string</returns>
        public string GetQuerystring()
        {
            if (QueryParameters == null || !QueryParameters.Any()) return "";

            string querystring = "?";

            KeyValuePair<string, string> last = QueryParameters.Last();

            foreach (var parameter in QueryParameters)
            {
                querystring = querystring + (parameter.Key + "=" + parameter.Value);
                if (!parameter.Equals(last))
                {
                    querystring += "&";
                }
            }

            return querystring;
        }

        /// <summary>
        ///     Clears the Query Parameters
        /// </summary>
        public void ClearQueryParameters()
        {
            QueryParameters = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        ///     Adds a query parameter so that when an appropriate request is issued a query string can be formed
        /// </summary>
        /// <param name="queryField">the Field name of a query field/value pairing</param>
        /// <param name="queryValue">the value of a query field/value pairing</param>
        public void AddQueryParameters(string queryField, string queryValue)
        {
            if (QueryParameters == null)
            {
                QueryParameters = new List<KeyValuePair<string, string>>();
            }

            QueryParameters.Add(new KeyValuePair<string, string>(queryField, queryValue));
        }

        /// <summary>
        ///     Adds a form parameter so that when necessary, form parameters can be populated for a HTTP request
        /// </summary>
        /// <param name="formName">The form name of the name/value pairing</param>
        /// <param name="formValue">The form value of the name/value pairing</param>
        public void AddFormParameter(string formName, string formValue)
        {
            if (FormParameters == null)
            {
                FormParameters = new Dictionary<string, string>();
            }

            FormParameters.Add(formName, formValue);
        }

        /// <summary>
        ///     Gets the form parameters
        /// </summary>
        /// <returns>FormURLEncoded Form parameters</returns>
        public HttpContent GetFormParameters()
        {
            List<KeyValuePair<string, string>> formBodyList = FormParameters.ToList();

            return new FormUrlEncodedContent(formBodyList);
        }

        /// <summary>
        ///     Clears the Form Parameters
        /// </summary>
        public void ClearFormParameters()
        {
            FormParameters = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Sets the json data based on a string input
        /// </summary>
        /// <param name="jsonData">The json data</param>
        public void SetJsonData(string jsonData)
        {
            JsonData = jsonData;
        }

        /// <summary>
        ///     Gets the json data that was set
        /// </summary>
        /// <returns>json data</returns>
        public string GetJsonData()
        {
            return JsonData;
        }


        /// <summary>
        ///     Clears the json data that was set
        /// </summary>
        public void ClearJsonData()
        {
            JsonData = null;
        }

        public String GetApiKey()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(AppKey + ":" + AppSecret);
            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        ///     Parses a multipart response into List of responses that can be accessed by index.
        /// </summary>
        /// <param name="multiResult">The multipart response that needs to be broken up into a list of responses</param>
        /// <returns>A List of responses from a multipart response</returns>
        public List<string> GetMultiPartResponses(string multiResult)
        {
            string[] output = Regex.Split(multiResult, "--Boundary([^;]+)");

            string[] splitString = output[1].Split(new[] {"--"}, StringSplitOptions.None);

            var responses = new List<string>();

            //We Can convert this to linq but for the sake of readability we'll leave it like this.
            foreach (string s in splitString)
            {
                if (s.Contains("{"))
                {
                    string json = s.Substring(s.IndexOf('{'));

                    JToken token = JObject.Parse(json);

                    responses.Add(token.ToString());
                }
            }

            return responses;
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        public void SetClient(HttpClient client)
        {
            _client = client;
        }
    }
}