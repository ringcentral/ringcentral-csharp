using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class AuthenticationTests : TestConfiguration
    {
        protected const string RefreshEndPoint = "/restapi/oauth/token";
        protected const string VersionEndPoint = "/restapi";
        protected const string RevokeEndPoint = "/restapi/oauth/revoke";

    

        [Test]
        public void TestAuthentication()
        {
            AuthResult = Platform.Authorize("username", "101", "password", true);
            Assert.NotNull(AuthResult);

            JToken token = JObject.Parse(AuthResult.GetBody());
            var accessToken = (string) token.SelectToken("access_token");
            var refreshToken = (string) token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);
        }

        [Test]
        public void TestRefresh()
        {
            
            Response refreshResult = Platform.Refresh();

            Assert.NotNull(refreshResult);

            JToken token = JObject.Parse(refreshResult.GetBody());
            var accessTokenAfterRefresh = (string) token.SelectToken("access_token");
            var refreshTokenAfterFresh = (string) token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenAfterRefresh);
            Assert.NotNull(refreshTokenAfterFresh);

        }

        [Test]
        public void TestVersion()
        {
            Request request = new Request(VersionEndPoint);
            Response response = Platform.Get(request);

            JToken token = response.GetJson();
            var version = (string) token.SelectToken("apiVersions").SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }

        [Test]
        public void RevokeAuthorization()
        {
            Response revokeResult = Platform.Logout();
            Assert.IsFalse(Platform.IsAuthorized());
        }

        [Test]
        public void GetAuthData()
        {
            AuthResult = Platform.Authorize("username", "101", "password", true);
            var authData = Platform.GetAuthData();
           
            JToken token = AuthResult.GetJson();

            Assert.AreEqual((string)token.SelectToken("access_token"),authData["access_token"]);
            Assert.AreEqual((string)token.SelectToken("refresh_token"),authData["refresh_token"]);




        }
    }
}