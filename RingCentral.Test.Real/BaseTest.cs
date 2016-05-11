using NUnit.Framework;
using System;
using System.Threading;

namespace RingCentral.Test
{
    [TestFixture]
    public class BaseTest
    {
        protected SDK sdk;

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

            if (appKey == "")
            { // don't run real tests if no appKey
                Assert.Ignore();
            }
            sdk = new SDK(appKey, appSecret, SDK.Server.Sandbox, "C Sharp Test Suite", "1.0.0");
            sdk.Platform.Authorize(username, extension, password, true);
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(3000); // there is a rate limit
        }
    }
}
