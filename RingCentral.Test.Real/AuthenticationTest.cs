using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class AuthenticationTest : BaseTest
    {
        [Test]
        public void GetToken()
        {
            // already authenticated in TestFixtureSetUp
            Assert.IsTrue(sdk.Platform.LoggedIn());
        }

        [Test]
        public void RevokeToken()
        {
            sdk.Platform.Logout();
            Assert.IsFalse(sdk.Platform.LoggedIn());
        }
    }
}
