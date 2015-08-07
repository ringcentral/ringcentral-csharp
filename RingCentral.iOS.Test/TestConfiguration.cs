using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Foundation;
using NUnit.Framework;
using RingCentral.Subscription;
using ObjCRuntime;
using UIKit;

namespace RingCentral.iOS.Test
{
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
        protected MockHttpClient MockResponseHandler = new MockHttpClient();

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

            RingCentralClient = new RingCentralClient(appKey, appSecret, ApiEndPoint);
            Platform = RingCentralClient.GetPlatform();
            Platform.SetClient(new HttpClient(MockResponseHandler) { BaseAddress = new Uri(ApiEndPoint) });
            AuthResult = Platform.Authenticate(UserName, password, Extension, true);
        }
    }
}