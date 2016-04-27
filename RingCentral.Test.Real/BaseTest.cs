using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;

namespace RingCentral.Test
{
    [TestFixture]
    public class BaseTest
    {
        private const string server = SDK.SANDBOX_SERVER;
        private SDK ringCentral;
        protected Platform platform;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var appKey = "";
            var appSecret = "";
            var username = "";
            var password = "";
            var extension = "";

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RC_APP_KEY")))
            {
                appKey = Environment.GetEnvironmentVariable("RC_APP_KEY");
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RC_APP_SECRET")))
            {
                appSecret = Environment.GetEnvironmentVariable("RC_APP_SECRET");
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RC_USERNAME")))
            {
                username = Environment.GetEnvironmentVariable("RC_USERNAME");
            }

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RC_PASSWORD")))
            {
                password = Environment.GetEnvironmentVariable("RC_PASSWORD");
            }

            ringCentral = new SDK(appKey, appSecret, server, "C Sharp Test Suite", "1.0.0");
            platform = ringCentral.GetPlatform();
            platform._client = new HttpClient(new WebRequestHandler()) { BaseAddress = new Uri(server) };
            platform.Authorize(username, extension, password, true);
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(3000); // there is a rate limit
        }
    }
}
