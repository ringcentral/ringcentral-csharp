using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;

namespace RingCentral.Test.Real
{
    [TestFixture]
    public class BaseTest
    {
        private const string ApiEndPoint = "https://platform.devtest.ringcentral.com";
        private RingCentral.SDK RingCentralClient;
        private Platform Platform;

        [OneTimeSetUp]
        public void SetUp()
        {
            var appKey = "";
            var appSecret = "";
            var username = "";
            var password = "";
            var extension = "";

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
                username = Environment.GetEnvironmentVariable("USER_NAME");
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PASSWORD")))
            {
                password = Environment.GetEnvironmentVariable("PASSWORD");
            }

            RingCentralClient = new RingCentral.SDK(appKey, appSecret, ApiEndPoint, "C Sharp Test Suite", "1.0.0");
            Platform = RingCentralClient.GetPlatform();
            Platform._client = new HttpClient(new WebRequestHandler()) { BaseAddress = new Uri(ApiEndPoint) };
            Platform.Authorize(username, extension, password, true);
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(10000); // there is a rate limit
        }
    }
}
