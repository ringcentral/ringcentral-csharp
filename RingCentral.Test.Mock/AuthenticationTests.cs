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
            var authResult = sdk.Platform.Login("username", "101", "password", true);
            Assert.NotNull(authResult);

            JToken token = JObject.Parse(authResult.Body);
            var accessToken = (string)token.SelectToken("access_token");
            var refreshToken = (string)token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);
        }

        [Test]
        public void TestRefresh()
        {

            var refreshResult = sdk.Platform.Refresh();

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
            var request = new Request(VersionEndPoint);
            var response = sdk.Platform.Get(request);

            JToken token = response.Json;
            var version = (string)token.SelectToken("apiVersions").First().SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }

        [Test]
        public void RevokeAuthorization()
        {
            sdk.Platform.Logout();
            Assert.IsFalse(sdk.Platform.LoggedIn());
        }

        [Test]
        public void GetAuthData()
        {
            var authResult = sdk.Platform.Login("username", "101", "password", true);
            var authData = sdk.Platform.Auth.GetData();

            JToken token = authResult.Json;

            Assert.AreEqual((string)token.SelectToken("access_token"), authData["access_token"]);
            Assert.AreEqual((string)token.SelectToken("refresh_token"), authData["refresh_token"]);
        }

        [Test]
        public void SetAuthData()
        {
            sdk.Platform.Login("username", "101", "password", true);
            var oldAuthData = sdk.Platform.Auth.GetData();

            var newAuthData = new Dictionary<string, string>
            {
                {"remember", "true"},
                {"token_type", "test"},
                {"owner_id", "test"},
                {"scope", "test"},
                {"access_token", oldAuthData["access_token"]},
                {"expires_in", oldAuthData["expires_in"]},
                {"refresh_token", oldAuthData["refresh_token"]},
                {"refresh_token_expires_in", oldAuthData["refresh_token_expires_in"]}
            };

            sdk.Platform.Auth.SetData(newAuthData);

            var authData = sdk.Platform.Auth.GetData();
            Debug.WriteLine(authData["access_token"]);
            Assert.AreEqual(newAuthData["access_token"], authData["access_token"]);
            Assert.AreEqual(newAuthData["refresh_token"], authData["refresh_token"]);
        }

        [Test]
        public void ShouldReturnAccessTokenValidGivenRestoredAuthData()
        {
            // Given
            var accessTokenExpireIn = DateTime.UtcNow.AddHours(1).Ticks/TimeSpan.TicksPerMillisecond;
            var refreshTokenExpireIn = DateTime.UtcNow.AddDays(7).Ticks / TimeSpan.TicksPerMillisecond;
            var data = new Dictionary<string, string>()
            {
                {"access_token", "ac1"},
                {"expires_in", "3598"},
                {"expire_time", accessTokenExpireIn.ToString()},
                {"refresh_token", "x"},
                {"refresh_token_expires_in", "604798"},
                {"refresh_token_expire_time", refreshTokenExpireIn.ToString()},
            };

            // When
            var auth = new Auth();
            auth.SetData(data);

            // Then expire_time should be set and token still valid
            var newData = auth.GetData();
            Assert.That(newData["expire_time"], Is.EqualTo(data["expire_time"]));
            Assert.That(auth.IsAccessTokenValid(), Is.EqualTo(true));
        }

        [Test]
        public void GenerateAuthorizeUri()
        {
            var authorizeUri = sdk.Platform.AuthorizeUri("http://localhost:3000", "myState");
            Assert.AreEqual(sdk.Platform.ServerUrl + "/restapi/oauth/authorize?response_type=code&state=myState&redirect_uri=http://localhost:3000&client_id=AppKey", authorizeUri);
        }

        [Test]
        public void AuthDataRefreshed()
        {
            var count = 0;
            sdk.Platform.AuthDataRefreshed += (sender, args) => {
                count += 1;
                var request = sender as Request;
                Assert.NotNull(request);
                Assert.NotNull(args.Response);
            };
            sdk.Platform.Refresh();
            Assert.AreEqual(1, count);
        }
    }
}