using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using RingCentral.Http;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class AuthenticationTests : TestConfiguration
    {
        protected const string RefreshEndPoint = "/restapi/oauth/token";
        protected const string VersionEndPoint = "/restapi";

    

        [Test]
        public void TestAuthentication()
        {
            AuthResult = Platform.Authenticate("username", "password", "101", true);
            Assert.NotNull(AuthResult);

            JToken token = JObject.Parse(AuthResult);
            var accessToken = (string) token.SelectToken("access_token");
            var refreshToken = (string) token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);
        }

        [Test]
        public void TestRefresh()
        {
            
            string refreshResult = Platform.Refresh();

            Assert.NotNull(refreshResult);

            JToken token = JObject.Parse(refreshResult);
            var accessTokenAfterRefresh = (string) token.SelectToken("access_token");
            var refreshTokenAfterFresh = (string) token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenAfterRefresh);
            Assert.NotNull(refreshTokenAfterFresh);

        }

        [Test]
        public void TestVersion()
        {
            Request request = new Request(VersionEndPoint);
            Response response = Platform.GetRequest(request);

            JToken token = response.GetJson();
            var version = (string) token.SelectToken("apiVersions").SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }
    }
}