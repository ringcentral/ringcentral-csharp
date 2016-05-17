using NUnit.Framework;
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
            if (Config.Instance == null)
            {
                Assert.Ignore();
            }
            sdk = new SDK(Config.Instance.AppKey, Config.Instance.AppSecret, Config.Instance.ServerUrl, "C Sharp Test Suite", "1.0.0");
            sdk.Platform.Login(Config.Instance.UserName, Config.Instance.Extension, Config.Instance.Password, true);
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(3000); // there is a rate limit
        }
    }
}
