using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RingCentral
{
    public class RingCentralClient
    {
        public RingCentralClient(String appKey, String appSecret, String apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
        }

        private String AppKey { get; set; }
        private String AppSecret { get; set; }
        private String ApiEndpoint { get; set; }
        private String AccessToken { get; set; }
        private String RefreshToken { get; set; }

        private List<KeyValuePair<String, String>> QueryParameters { get; set; }
        private Dictionary<String, String> FormParameters { get; set; }
        private String JsonData { get; set; }


        public String Authenticate(string userName, String password, String extension)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                FormParameters = new Dictionary<String, String>
                                 {
                                     {"username", userName},
                                     {"password", Uri.EscapeUriString(password)},
                                     {"extension", extension},
                                     {"grant_type", "password"}
                                 };

                string result = AuthPostRequest("/restapi/oauth/token");

                JToken token = JObject.Parse(result);
                AccessToken = (String) token.SelectToken("access_token");
                RefreshToken = (String) token.SelectToken("refresh_token");

                return result;
            }
        }

        public String Refresh(String request)
        {
            FormParameters = new Dictionary<String, String>
                             {
                                 {"grant_type", "refresh_token"},
                                 {"refresh_token", RefreshToken}
                             };

            string result = AuthPostRequest(request);

            JToken token = JObject.Parse(result);

            AccessToken = (String) token.SelectToken("access_token");
            RefreshToken = (String) token.SelectToken("refresh_token");

            return result;
        }

        public String Revoke(String request)
        {
            FormParameters = new Dictionary<String, String>
                             {
                                 {"token", AccessToken}
                             };

            return AuthPostRequest(request);
        }

        public String PostRequest(String request)
        {
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

        public String AuthPostRequest(String request)
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

        public String GetRequest(String request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                request += GetQueryString();

                Task<HttpResponseMessage> accountResult = client.GetAsync(request);

                ClearQueryParameters();

                return accountResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public String DeleteRequest(String request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                request += GetQueryString();

                Task<HttpResponseMessage> accountResult = client.DeleteAsync(request);

                return accountResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public String PutRequest(String request)
        {
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

                request += GetQueryString();

                Task<HttpResponseMessage> accountResult = client.PutAsync(request, httpContent);

                return accountResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        //TODO: need to implement, returns based on token expiration time
        public Boolean IsAuthorized()
        {
            return true;
        }

        public String GetQueryString()
        {
            if (QueryParameters == null || !QueryParameters.Any()) return "";

            string queryString = "?";

            KeyValuePair<string, string> last = QueryParameters.Last();

            foreach (var parameter in QueryParameters)
            {
                queryString = queryString + (parameter.Key + "=" + parameter.Value);
                if (!parameter.Equals(last))
                {
                    queryString += "&";
                }
            }

            return queryString;
        }

        public void ClearQueryParameters()
        {
            QueryParameters = new List<KeyValuePair<string, string>>();
        }

        public void AddQueryParameters(String queryField, String queryValue)
        {
            if (QueryParameters == null)
            {
                QueryParameters = new List<KeyValuePair<string, string>>();
            }

            QueryParameters.Add(new KeyValuePair<string, string>(queryField, queryValue));
        }

        public void AddFormParameter(String formName, String formValue)
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

        public void SetJsonData(String jsonData)
        {
            JsonData = jsonData;
        }

        public String GetJsonData()
        {
            return JsonData;
        }

        public void ClearJsonDat()
        {
            JsonData = null;
        }
    }
}