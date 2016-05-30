using NUnit.Framework;
using RingCentral.Subscription;

namespace RingCentral.Test
{
    class SubscriptionServiceTests
    {
        [TestFixture]
        public class PubNubSubscriptionTest : BaseTest
        {
            [Test]
            public void SubscribeTest()
            {
                var sub = sdk.CreateSubscription();
                sub.EventFilters.Add("/restapi/v1.0/account/~/extension/~/presence");
                sub.EventFilters.Add("/restapi/v1.0/account/~/extension/~/mesage-store");
                sub.Register();
                Assert.AreEqual(true, sub.Alive());
                sub.Register();
                Assert.AreEqual(true, sub.Alive());
                sub.Remove();
                Assert.AreEqual(false, sub.Alive());
            }

            [Test]
            public void SubscriptionEventArgsTest()
            {
                var eventArgs = new SubscriptionEventArgs("Hello world");
                Assert.AreEqual("Hello world", eventArgs.Message);
            }
        }
    }
}
