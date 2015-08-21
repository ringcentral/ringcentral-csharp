using System;
using System.Net.Http;
using NUnit.Framework;
using RingCentral.SDK.Http;
using RingCentral.SDK;

namespace RingCentral.Test
{
    [TestFixture]
    public class TestConfiguration
    {

        protected string UserName = "";

        protected const string Extension = "101";

        protected const string ApiEndPoint = "https://platform.devtest.ringcentral.com";

        protected const string RevokeEndPoint = "/restapi/oauth/revoke";

        protected const string AuthenticateEndPoint = "/restapi/oauth/token";

        protected const string SmsText = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

        protected string ToPhone = "";

        protected Response AuthResult;

        protected Platform Platform;

        protected RingCentral.SDK.SDK RingCentralClient;
        protected MockHttpClient MockResponseHandler = new MockHttpClient();

        [TestFixtureSetUp]
        public void SetUp()
        {

            var appKey = "";
            var appSecret = "";
            var password = "";

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_KEY")))
            {

                appKey = Environment.GetEnvironmentVariable("APP_KEY");
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_SECRET")))
            {
                appSecret = Environment.GetEnvironmentVariable("APP_SECRET");
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("USER_NAME")))
            {
                UserName = Environment.GetEnvironmentVariable("USER_NAME");
                ToPhone = UserName;
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PASSWORD")))
            {
                password = Environment.GetEnvironmentVariable("PASSWORD");
            }
           
            RingCentralClient = new RingCentral.SDK.SDK(appKey, appSecret, ApiEndPoint);
            Platform = RingCentralClient.GetPlatform();
            Platform._client = new HttpClient(MockResponseHandler) { BaseAddress = new Uri(ApiEndPoint) };
            AuthResult = Platform.Authorize(UserName, Extension, password, true);
        }

      
    }
}