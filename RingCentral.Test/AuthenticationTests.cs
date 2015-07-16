using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RingCentral.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationTests : TestConfiguration
    {

        [Test]
        public void TestAuthentication()
        {
            var result = RingCentralClient.Authenticate(UserName, Password, Extension);

            Assert.NotNull(result);

            JToken token = JObject.Parse(result);
            var accessToken = (String)token.SelectToken("access_token");
            var refreshToken = (String)token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);
        }

        [Test]
        public void TestRefresh()
        {
            
            var authenticateResult = RingCentralClient.Authenticate(UserName, Password, Extension);

            Assert.NotNull(authenticateResult);

            JToken token = JObject.Parse(authenticateResult);
            var accessTokenBeforeRefresh = (String)token.SelectToken("access_token");
            var refreshTokenBeforeFresh = (String)token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenBeforeRefresh);
            Assert.NotNull(refreshTokenBeforeFresh);

            var refreshResult = RingCentralClient.Refresh(RefreshEndPoint);

            Assert.NotNull(refreshResult);

            token = JObject.Parse(refreshResult);
            var accessTokenAfterRefresh = (String)token.SelectToken("access_token");
            var refreshTokenAfterFresh = (String)token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenAfterRefresh);
            Assert.NotNull(refreshTokenAfterFresh);

            Assert.AreNotEqual(accessTokenBeforeRefresh, accessTokenAfterRefresh);
            Assert.AreNotEqual(refreshTokenBeforeFresh, refreshTokenAfterFresh);
        }

        [Test]
        public void TestVersion()
        {
            var result = RingCentralClient.GetRequest(VersionEndPoint);

            JToken token = JObject.Parse(result);
            var version = (String)token.SelectToken("apiVersions")[0].SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }
    }
}
