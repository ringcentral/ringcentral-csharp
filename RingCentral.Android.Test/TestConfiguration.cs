using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NUnit.Framework;
using RingCentral.Http;
using Environment = System.Environment;

namespace RingCentral.Android.Test
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

        protected Response AuthResult;

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

            RingCentralClient = new RingCentralClient(appKey, appSecret, ApiEndPoint);
            Platform = RingCentralClient.GetPlatform();
            Platform.SetClient(new HttpClient(mockResponseHandler) { BaseAddress = new Uri(ApiEndPoint) });
            AuthResult = Platform.Authenticate(UserName, password, Extension, true);
        }


    }
}