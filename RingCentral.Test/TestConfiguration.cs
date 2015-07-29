using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using System.Net.Http;

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

        protected const string ToPhone = "***REMOVED***";

        protected string AuthResult;

        protected Platform Platform;

        protected RingCentralClient RingCentralClient;
        protected MockHttpClient mockResponseHandler = new MockHttpClient();

        [TestFixtureSetUp]
        public void SetUp()
        {

            var appKey = "";
            var appSecret = "";
            var password = "";

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_KEY")))
            {

                appKey = Environment.GetEnvironmentVariable("APP_KEY");
            }

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_SECRET")))
            {
                appSecret = Environment.GetEnvironmentVariable("APP_SECRET");
            }

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("USER_NAME")))
            {
                UserName = Environment.GetEnvironmentVariable("USER_NAME");
            }

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PASSWORD")))
            {
                password = Environment.GetEnvironmentVariable("PASSWORD");
            }


            //TODO: we'll have these removed and drive the application credentials via environment variables
            //appKey = "***REMOVED***";
            //appSecret = "***REMOVED***";
            //UserName = "***REMOVED***";
            //password = "***REMOVED***";

            RingCentralClient = new RingCentralClient(appKey, appSecret, ApiEndPoint);
            AuthResult = RingCentralClient.GetPlatform().Authenticate(UserName, password, Extension, AuthenticateEndPoint);
            Platform = RingCentralClient.GetPlatform();
            Platform.SetClient(new HttpClient(mockResponseHandler) { BaseAddress = new Uri(ApiEndPoint) });
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            //RingCentralClient.GetPlatform().Revoke(RevokeEndPoint);
            //RingCentralClient = null;
            //Due to Request limitions a wait of 25 second is needed to sure not to exceed the maximum requst rate / minute
            //Thread.Sleep(25000);
        }
    }
}