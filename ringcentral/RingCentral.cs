using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;


namespace ringcentral
{
    public class RingCentral
    {

        private String AppKey { get; set; }
        private String AppSecret { get; set; }
        private String ApiEndpoint { get; set; }
        private String AccessToken { get; set; }

        public RingCentral(String appKey, String appSecret, String apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
        }


        public String Authenticate(string userName, String password, String extension, String grantType)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiEndpoint);

                var formBodyContent = new Dictionary<String, String>
                                      {
                                          { "token", AccessToken }, 
                                          { "username", userName }, 
                                          { "password", Uri.EscapeUriString(password) }, 
                                          { "extension", extension }, 
                                          { "grant_type", grantType }
                                      };


               return PostRequest("/restapi/oauth/token", formBodyContent);

            }
        }

        public String Revoke()
        {

            var formBodyContent = new Dictionary<String, String> {{"token", AccessToken}};

            return PostRequest("/restapi/oauth/revoke", formBodyContent);

        }

        public String PostRequest(String request, Dictionary<String, String> formContent )
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(ApiEndpoint);

                var byteArray = Encoding.UTF8.GetBytes(AppKey + ":" + AppSecret);

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var formBodyList = formContent.ToList();

                formBodyList.Add(new KeyValuePair<string, string>("token", AccessToken));

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
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

                var accountResult = client.GetAsync(request);

                return accountResult.Result.Content.ReadAsStringAsync().Result;

            }
        }
    }
}
