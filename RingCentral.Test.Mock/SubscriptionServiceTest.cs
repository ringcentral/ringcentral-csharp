using NUnit.Framework;
using RingCentral.Subscription;
using System.Collections.Generic;
using System.Threading;

namespace RingCentral.Test
{
    class SubscriptionServiceTests
    {
        [TestFixture]
        public class PubNubSubscriptionTest : BaseTest
        {
            //private const string Channel = "RCNETSDK-TEST";

            //[Test]
            //public void SubscribeTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
            //    sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
            //    var subscribed = sub.Subscribe(null, null, null);
            //    Thread.Sleep(1000);
            //    Assert.IsNotNull(subscribed);
            //    Assert.AreEqual(true, subscribed.OK);
            //    Assert.IsTrue(sub.IsSubscribed());
            //    sub.Remove();
            //    Thread.Sleep(1000);
            //}

            //[Test]
            //public void RenewSubscribeTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
            //    sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
            //    sub.Subscribe(null, null, null);
            //    Thread.Sleep(500);
            //    sub.ClearEvents();
            //    sub.Events = new List<string>() { "/restapi/v1.0/account/~/extension/~/presence" };
            //    sub.Renew();
            //    Assert.IsTrue(sub.IsSubscribed());
            //    sub.Remove();
            //    Thread.Sleep(500);
            //}

            //[Test]
            //public void DeleteSubscribeTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
            //    sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
            //    sub.Subscribe(null, null, null);
            //    Thread.Sleep(500);
            //    sub.Remove();
            //    Assert.IsFalse(sub.IsSubscribed());
            //    Thread.Sleep(500);
            //}

            //[Test]
            //[ExpectedException(typeof(System.Exception), ExpectedMessage = "Event filters are undefined")]
            //public void NoEventFiltersTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.Subscribe(null, null, null);
            //    Thread.Sleep(500);
            //}

            //[Test]
            //[ExpectedException(typeof(System.Exception), ExpectedMessage = "Subscription ID is required")]
            //public void NoSubscriptionIdRenewTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.Renew();
            //    Thread.Sleep(500);
            //}

            //[Test]
            //[ExpectedException(typeof(System.Exception), ExpectedMessage = "Subscription ID is required")]
            //public void NoSubscriptionIdRemoveTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.Remove();
            //    Thread.Sleep(500);
            //}

            //[Test]
            //public void SetSslFlagForPubnubTest()
            //{
            //    SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() { _platform = sdk.Platform };
            //    sub.EnableSSL(true);
            //    Assert.True(sub.IsSSL());
            //}
        }
    }
}
