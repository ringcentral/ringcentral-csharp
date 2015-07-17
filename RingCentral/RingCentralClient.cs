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

namespace RingCentral
{
    public class RingCentralClient
    {
        public RingCentralClient(string appKey, string appSecret, string apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
        }

        private string AppKey { get; set; }
        private string AppSecret { get; set; }
        private string ApiEndpoint { get; set; }
        private string AccessToken { get; set; }
        private string RefreshToken { get; set; }
        
        private long AccessTokenExpiresIn { get; set; }
        private long AccessTokenExpireTime { get; set; }
        private long RefreshTokenExpiresIn { get; set; }
        private long RefreshTokenExpireTime { get; set; }

        private List<KeyValuePair<string, string>> QueryParameters { get; set; }
        private Dictionary<string, string> FormParameters { get; set; }
        private string JsonData { get; set; }

        public bool IsMultiPartResponse { get; set; }


        public string Authenticate(string userName, string password, string extension, string endPoint)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                FormParameters = new Dictionary<string, string>
                                 {
                                     {"username", userName},
                                     {"password", Uri.EscapeUriString(password)},
                                     {"extension", extension},
                                     {"grant_type", "password"}
                                 };

                string result = AuthPostRequest(endPoint);

                JToken token = JObject.Parse(result);
               
                AccessToken = (string) token.SelectToken("access_token");
                RefreshToken = (string) token.SelectToken("refresh_token");
                AccessTokenExpiresIn = (long) token.SelectToken("expires_in");
                RefreshTokenExpiresIn = (long) token.SelectToken("refresh_token_expires_in");

                var currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                AccessTokenExpireTime = (AccessTokenExpiresIn + currentTimeInMilliseconds);
                                
                RefreshTokenExpireTime = (RefreshTokenExpiresIn + currentTimeInMilliseconds);

                return result;
            }
        }

        public string Refresh(string request)
        {
            if (!IsRefreshTokenValid()) throw new Exception("Refresh Token has Expired");
            
            FormParameters = new Dictionary<string, string>
                             {
                                 {"grant_type", "refresh_token"},
                                 {"refresh_token", RefreshToken}
                             };

            string result = AuthPostRequest(request);

            JToken token = JObject.Parse(result);

            AccessToken = (string) token.SelectToken("access_token");
            RefreshToken = (string) token.SelectToken("refresh_token");
            AccessTokenExpiresIn = (long) token.SelectToken("expires_in");
            RefreshTokenExpiresIn = (long) token.SelectToken("refresh_token_expires_in");

            var currentTimeInMilliseconds = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;

            AccessTokenExpireTime = (AccessTokenExpiresIn + currentTimeInMilliseconds);

            RefreshTokenExpireTime = (RefreshTokenExpiresIn + currentTimeInMilliseconds);

            return result;
        }

        public string Revoke(string request)
        {
            FormParameters = new Dictionary<string, string>
                             {
                                 {"token", AccessToken}
                             };

            return AuthPostRequest(request);
        }

        public string AuthPostRequest(string request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                byte[] byteArray = Encoding.UTF8.GetBytes(AppKey + ":" + AppSecret);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage result = client.PostAsync(request, GetFormParameters()).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public string PostRequest(string request)
        {
            if (!IsAccessTokenValid()) throw new Exception("Access has Expired");

            using (var client = new HttpClient())
            {
                HttpContent httpContent = null;

                client.BaseAddress = new Uri(ApiEndpoint);

                if (JsonData != null)
                {
                    httpContent = new StringContent(JsonData, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                }
                else
                {
                    httpContent = GetFormParameters();
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);


                HttpResponseMessage result = client.PostAsync(request, httpContent).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public string GetRequest(string request)
        {
            if (!IsAccessTokenValid()) throw new Exception("Access has Expired");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                request += GetQuerystring();

                Task<HttpResponseMessage> getResult = client.GetAsync(request);

                var contentType = getResult.Result.Content.Headers.ContentType;
                Debug.WriteLine("Content type in response is: " + contentType);

                if (contentType.ToString().Contains("multipart/mixed"))
                {
                    IsMultiPartResponse = true;
                }

                ClearQueryParameters();

                return getResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public string DeleteRequest(string request)
        {
            if (!IsAccessTokenValid()) throw new Exception("Access has Expired");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                request += GetQuerystring();

                Task<HttpResponseMessage> accountResult = client.DeleteAsync(request);

                return accountResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public string PutRequest(string request)
        {
            if (!IsAccessTokenValid()) throw new Exception("Access has Expired");

            using (var client = new HttpClient())
            {
                HttpContent httpContent;

                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                if (JsonData != null)
                {
                    httpContent = new StringContent(JsonData, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                }
                else
                {
                    httpContent = GetFormParameters();
                }

                request += GetQuerystring();

                Task<HttpResponseMessage> accountResult = client.PutAsync(request, httpContent);

                return accountResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public bool IsTokenValid(long accessToken)
        {
            var currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            return accessToken > currentTimeInMilliseconds;
        }

        public bool IsAccessTokenValid()
        {
            return IsTokenValid(AccessTokenExpireTime);
        }

        public bool IsRefreshTokenValid()
        {
            return IsTokenValid(RefreshTokenExpireTime);
        }

        public List<string> GetMultiPartResponses(string multiResult)
        {
            var output = Regex.Split(multiResult, "--Boundary([^;]+)");

            var splitString = output[1].Split(new[] { "--" }, StringSplitOptions.None);
            var responses = new List<string>();
            foreach (var s in splitString)
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

        public void ClearQueryParameters()
        {
            QueryParameters = new List<KeyValuePair<string, string>>();
        }

        public void AddQueryParameters(string queryField, string queryValue)
        {
            if (QueryParameters == null)
            {
                QueryParameters = new List<KeyValuePair<string, string>>();
            }

            QueryParameters.Add(new KeyValuePair<string, string>(queryField, queryValue));
        }

        public void AddFormParameter(string formName, string formValue)
        {
            if (FormParameters == null)
            {
                FormParameters = new Dictionary<string, string>();
            }

            FormParameters.Add(formName, formValue);
        }

        public HttpContent GetFormParameters()
        {
            List<KeyValuePair<string, string>> formBodyList = FormParameters.ToList();

            return new FormUrlEncodedContent(formBodyList);
        }

        public void ClearFormParameters()
        {
            FormParameters = new Dictionary<string, string>();
        }

        public void SetJsonData(string jsonData)
        {
            JsonData = jsonData;
        }

        public string GetJsonData()
        {
            return JsonData;
        }

        public void ClearJsonDat()
        {
            JsonData = null;
        }

    }
}