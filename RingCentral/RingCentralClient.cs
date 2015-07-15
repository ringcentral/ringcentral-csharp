using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RingCentral
{
    public class RingCentralClient
    {
        private String AppKey { get; set; }
        private String AppSecret { get; set; }
        private String ApiEndpoint { get; set; }
        private String AccessToken { get; set; }
        private String RefreshToken { get; set; }

        public RingCentralClient(String appKey, String appSecret, String apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
        }


        public String Authenticate(string userName, String password, String extension)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                var formBodyContent = new Dictionary<String, String>
                                      {
                                          { "username", userName }, 
                                          { "password", Uri.EscapeUriString(password) }, 
                                          { "extension", extension }, 
                                          { "grant_type", "password" }
                                      };

                var result = AuthPostRequest("/restapi/oauth/token", formBodyContent);

                JToken token = JObject.Parse(result);
                AccessToken = (String)token.SelectToken("access_token");
                RefreshToken = (String)token.SelectToken("refresh_token");

                return result;

            }
        }

        public String Refresh(String request)
        {

            var formBodyContent = new Dictionary<String, String> { { "grant_type", "refresh_token" }, { "refresh_token", RefreshToken } };

            var result = AuthPostRequest(request, formBodyContent);

            JToken token = JObject.Parse(result);

            AccessToken = (String)token.SelectToken("access_token");
            RefreshToken = (String)token.SelectToken("refresh_token");

            return result;

        }

        public String Revoke(String request)
        {

            var formBodyContent = new Dictionary<String, String> { { "token", AccessToken } };

            return AuthPostRequest(request, formBodyContent);

        }

        public String PostRequest(String request, Dictionary<String, String> formContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var formBodyList = formContent.ToList();

                var content = new FormUrlEncodedContent(formBodyList);

                var result = client.PostAsync(request, content).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public String PostRequest(String request, String jsonData)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                var result = client.PostAsync(request, new StringContent(jsonData, Encoding.UTF8, "application/json")).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public String AuthPostRequest(String request, Dictionary<String, String> formContent)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(ApiEndpoint);

                var byteArray = Encoding.UTF8.GetBytes(AppKey + ":" + AppSecret);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var formBodyList = formContent.ToList();

                var content = new FormUrlEncodedContent(formBodyList);

                var result = client.PostAsync(request, content).Result;

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

                var accountResult = client.GetAsync(request);

                return accountResult.Result.Content.ReadAsStringAsync().Result;

            }
        }

        //TODO: some get requests have query parameters in the request, need to add to client.GetAsync if present
        public String GetRequest(String request,Dictionary<String,String> queryData )
        {
            using (var client = new HttpClient())
            {
                //var uriBuilder = new UriBuilder();

                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                var accountResult = client.GetAsync(request);

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

                var accountResult = client.DeleteAsync(request);

                return accountResult.Result.Content.ReadAsStringAsync().Result;

            }
        }

        public String PutRequest(String request, Dictionary<String, String> formContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                var formBodyList = formContent.ToList();

                var content = new FormUrlEncodedContent(formBodyList);

                var accountResult = client.PutAsync(request, content);

                return accountResult.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public String PutRequest(String request, String jsonData)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(ApiEndpoint);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                var result = client.PutAsync(request, new StringContent(jsonData, Encoding.UTF8, "application/json")).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        //TODO: need to implement, returns based on token expiration time
        public Boolean IsAuthorized()
        {
            return true;
        }
    }
}
