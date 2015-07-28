using Newtonsoft.Json.Linq;
using NUnit.Framework;

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
            Assert.NotNull(AuthResult);

            JToken token = JObject.Parse(AuthResult);
            var accessTokenBeforeRefresh = (string) token.SelectToken("access_token");
            var refreshTokenBeforeFresh = (string) token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenBeforeRefresh);
            Assert.NotNull(refreshTokenBeforeFresh);

            string refreshResult = RingCentralClient.GetPlatform().Refresh(RefreshEndPoint);

            Assert.NotNull(refreshResult);

            token = JObject.Parse(refreshResult);
            var accessTokenAfterRefresh = (string) token.SelectToken("access_token");
            var refreshTokenAfterFresh = (string) token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenAfterRefresh);
            Assert.NotNull(refreshTokenAfterFresh);

            Assert.AreNotEqual(accessTokenBeforeRefresh, accessTokenAfterRefresh);
            Assert.AreNotEqual(refreshTokenBeforeFresh, refreshTokenAfterFresh);
        }

        [Test]
        public void TestVersion()
        {
            string result = RingCentralClient.GetPlatform().GetRequest(VersionEndPoint);

            JToken token = JObject.Parse(result);
            var version = (string) token.SelectToken("apiVersions")[0].SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }
    }
}