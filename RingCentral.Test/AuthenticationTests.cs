using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class AuthenticationTests : TestConfiguration
    {
        protected const string RefreshEndPoint = "/restapi/oauth/token";
        protected const string VersionEndPoint = "/restapi";

        [TestFixtureSetUp]
        public void Setup()
        {
            mockResponseHandler.AddPostMockResponse(
                new Uri(ApiEndPoint + RefreshEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        "{\"access_token\": \"abcdefg\",\"token_type\": \"bearer\",\"expires_in\": 3599, \"refresh_token\": \"gfedcba\",\"refresh_token_expires_in\": 604799," + 
                        "\"scope\": \"EditCustomData EditAccounts ReadCallLog EditPresence SMS Faxes ReadPresence ReadAccounts Contacts EditExtensions InternalMessages EditMessages ReadCallRecording ReadMessages EditPaymentInfo EditCallLog NumberLookup Accounts RingOut ReadContacts\","+
                        "\"owner_id\": \"1\" }"
                        )
                });
            mockResponseHandler.AddGetMockResponse(
                new Uri(ApiEndPoint + VersionEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{ \"apiVersions\": { \"uriString\": \"v1.0\" } }" )
                });
        }

        [Test]
        public void TestAuthentication()
        {
            AuthResult = Platform.Authenticate("username", "password", "101", RefreshEndPoint);
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
            
            string refreshResult = Platform.Refresh(RefreshEndPoint);

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
            string result = Platform.GetRequest(VersionEndPoint);

            JToken token = JObject.Parse(result);
            var version = (string) token.SelectToken("apiVersions").SelectToken("uriString");

            Assert.AreEqual(version, "v1.0");
        }
    }
}