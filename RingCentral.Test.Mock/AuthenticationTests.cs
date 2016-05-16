using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RingCentral.Test
{

    public partial class MockHttpClient
    {
        private void AddAuthenticationMockData()
        {
            mockResponses[HttpMethod.Post][new Uri(ServerUrl + "/restapi/oauth/token")] = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
   ""access_token"" : ""U1BCMDFUMDRKV1MwMXxzLFSvXdw5PHMsVLEn_MrtcyxUsw"",
   ""token_type"" : ""bearer"",
   ""expires_in"" : 7199,
   ""refresh_token"" : ""U1BCMDFUMDRKV1MwMXxzLFL4ec6A0XMsUv9wLriecyxS_w"",
   ""refresh_token_expires_in"" : 604799,
   ""scope"" : ""AccountInfo CallLog ExtensionInfo Messages SMS"",
   ""owner_id"" : ""256440016""
}", Encoding.UTF8, "application/json")
            };

            mockResponses[HttpMethod.Post][new Uri(ServerUrl + "/restapi/oauth/revoke")] = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            mockResponses[HttpMethod.Get][new Uri(ServerUrl + "/restapi")] = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
  ""uri"" : ""https.../restapi/"",
  ""apiVersions"" : [ {
    ""uri"" : ""https.../restapi/v1.0"",
    ""versionString"" : ""1.0.14"",
    ""releaseDate"" : ""2014-10-31T00:00:00.000Z"",
    ""uriString"" : ""v1.0""
  } ],
  ""serverVersion"" : ""7.0.0.551"",
  ""serverRevision"" : ""598ed4edcc56""
}", Encoding.UTF8, "application/json")
            };
        }
    }

    [TestFixture]
    public class AuthenticationTests : BaseTest
    {
        protected const string RefreshEndPoint = "/restapi/oauth/token";
        protected const string VersionEndPoint = "/restapi";



        [Test]
        public void TestAuthentication()
        {
            var AuthResult = sdk.Platform.Login("username", "101", "password", true);
            Assert.NotNull(AuthResult);

            JToken token = JObject.Parse(AuthResult.Body);
            var accessToken = (string)token.SelectToken("access_token");
            var refreshToken = (string)token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);
        }

        [Test]
        public void TestRefresh()
        {

            ApiResponse refreshResult = sdk.Platform.Refresh();

            Assert.NotNull(refreshResult);

            JToken token = JObject.Parse(refreshResult.Body);
            var accessTokenAfterRefresh = (string)token.SelectToken("access_token");
            var refreshTokenAfterFresh = (string)token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenAfterRefresh);
            Assert.NotNull(refreshTokenAfterFresh);

        }

        [Test]
        public void TestVersion()
        {
            Request request = new Request(VersionEndPoint);
            ApiResponse response = sdk.Platform.Get(request);

            JToken token = response.Json;
            var version = (string)token.SelectToken("apiVersions").First().SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }

        [Test]
        public void RevokeAuthorization()
        {
            ApiResponse revokeResult = sdk.Platform.Logout();
            Assert.IsFalse(sdk.Platform.LoggedIn());
        }

        [Test]
        public void GetAuthData()
        {
            var AuthResult = sdk.Platform.Login("username", "101", "password", true);
            var authData = sdk.Platform.Auth.GetData();

            JToken token = AuthResult.Json;

            Assert.AreEqual((string)token.SelectToken("access_token"), authData["access_token"]);
            Assert.AreEqual((string)token.SelectToken("refresh_token"), authData["refresh_token"]);
        }

        [Test]
        public void SetAuthData()
        {
            var AuthResult = sdk.Platform.Login("username", "101", "password", true);
            var oldAuthData = sdk.Platform.Auth.GetData();

            var newAuthData = new Dictionary<string, string>();
            newAuthData.Add("remember", "true");
            newAuthData.Add("token_type", "test");
            newAuthData.Add("owner_id", "test");
            newAuthData.Add("scope", "test");
            newAuthData.Add("access_token", oldAuthData["access_token"]);
            newAuthData.Add("expires_in", oldAuthData["expires_in"]);
            newAuthData.Add("refresh_token", oldAuthData["refresh_token"]);
            newAuthData.Add("refresh_token_expires_in", oldAuthData["refresh_token_expires_in"]);

            sdk.Platform.Auth.SetData(newAuthData);

            var authData = sdk.Platform.Auth.GetData();
            Debug.WriteLine(authData["access_token"]);
            Assert.AreEqual(newAuthData["access_token"], authData["access_token"]);
            Assert.AreEqual(newAuthData["refresh_token"], authData["refresh_token"]);
        }
    }
}