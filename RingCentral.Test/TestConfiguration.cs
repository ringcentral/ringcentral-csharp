using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using System.Net.Http;
using System.Net;

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
            mockResponseHandler.AddPostMockResponse(
               new Uri(ApiEndPoint + AuthenticateEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(
                       "{\"access_token\": \"abcdefg\",\"token_type\": \"bearer\",\"expires_in\": 3599, \"refresh_token\": \"gfedcba\",\"refresh_token_expires_in\": 604799," +
                       "\"scope\": \"EditCustomData EditAccounts ReadCallLog EditPresence SMS Faxes ReadPresence ReadAccounts Contacts EditExtensions InternalMessages EditMessages ReadCallRecording ReadMessages EditPaymentInfo EditCallLog NumberLookup Accounts RingOut ReadContacts\"," +
                       "\"owner_id\": \"1\" }"
                       )
               });
            RingCentralClient = new RingCentralClient(appKey, appSecret, ApiEndPoint);
            Platform = RingCentralClient.GetPlatform();
            Platform.SetClient(new HttpClient(mockResponseHandler) { BaseAddress = new Uri(ApiEndPoint) });
            AuthResult = Platform.Authenticate(UserName, password, Extension, true);
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