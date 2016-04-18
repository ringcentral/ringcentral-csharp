using NUnit.Framework;

namespace RingCentral.Test.Real
{
    [TestFixture]
    public class AuthenticationTest : BaseTest
    {
        [Test]
        public void GetToken()
        {
            // already authenticated in OneTimeSetUp
            Assert.IsTrue(platform.LoggedIn());
        }

        [Test]
        public void RevokeToken()
        {
            platform.Logout();
            Assert.IsFalse(platform.LoggedIn());
        }
    }
}
