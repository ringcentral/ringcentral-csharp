using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using RingCentral.Subscription;

namespace RingCentral.Test
{
    class SubscriptionServiceTests
    {
        [TestFixture]
        public class PubNubSubscriptionTest : RingCentral.Test.TestConfiguration
        {

            private const string Channel = "RCNETSDK-TEST";
            private SubscriptionServiceMock _subscriptionServiceMock;


            [Test]
            public void UnsubscribePubNubTest()
            {
                _subscriptionServiceMock = new SubscriptionServiceMock("demo-36", "demo-36", "demo-36", "", false);
                _subscriptionServiceMock.Subscribe(Channel + "2", "", null, null, null);
                Thread.Sleep(500);
                _subscriptionServiceMock.Unsubscribe(Channel + "2", "", null, null, null, null);
                Thread.Sleep(500);
                Assert.IsTrue(
                    _subscriptionServiceMock.ReturnMessage("disconnectMessage")
                        .ToString()
                        .Contains("Channel Unsubscribed from RCNETSDK-TEST2"));
                Thread.Sleep(500);

            }

            [Test]
            public void ErrorMessagePubNubTest()
            {
                _subscriptionServiceMock = new SubscriptionServiceMock("demo-36", "demo-36", "demo-36", "", false);
                _subscriptionServiceMock.Subscribe(Channel + "3", "", null, null, null);
                Thread.Sleep(500);
                _subscriptionServiceMock.Subscribe(Channel + "3", "", null, null, null);
                Thread.Sleep(500);
                Assert.IsTrue(
                    _subscriptionServiceMock.ReturnMessage("errorMessage")
                        .ToString()
                        .Contains("Channel Already Subscribed. Duplicate channel subscription not allowed"));
                Thread.Sleep(500);
            }


            [Test]
            public void SubscribeTest()
            {
                SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() {_platform = Platform};
                sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
                sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
                var subscribed = sub.Subscribe(null, null, null);
                Thread.Sleep(1000);
                Assert.IsNotNull(subscribed);
                Assert.AreEqual(true, subscribed.CheckStatus());
                Assert.IsTrue(sub.IsSubscribed());
                sub.Remove();
                Thread.Sleep(1000);

            }

            [Test]
            public void RenewSubscribeTest()
            {
                SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() {_platform = Platform};
                sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
                sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
                var test = sub.Subscribe(null, null, null);
                Thread.Sleep(500);
                sub.ClearEvents();
                sub.SetEvents(new List<string>() {"/restapi/v1.0/account/~/extension/~/presence"});
                sub.Renew();
                Assert.IsTrue(sub.IsSubscribed());
                sub.Remove();
                Thread.Sleep(500);
            }

            [Test]
            public void DeleteSubscribeTest()
            {

                SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() {_platform = Platform};
                sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
                sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
                var test = sub.Subscribe(null, null, null);
                Thread.Sleep(500);
                sub.Remove();
                Assert.IsFalse(sub.IsSubscribed());
                Thread.Sleep(500);

            }

            [Test]
            [ExpectedException(typeof (System.Exception), ExpectedMessage = "Event filters are undefined")]
            public void NoEventFiltersTest()
            {
                SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() {_platform = Platform};
                sub.Subscribe(null, null, null);
                Thread.Sleep(500);
            }

            [Test]
            [ExpectedException(typeof (System.Exception), ExpectedMessage = "Subscription ID is required")]
            public void NoSubscriptionIdRenewTest()
            {
                SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() {_platform = Platform};
                sub.Renew();
                Thread.Sleep(500);
            }

            [Test]
            [ExpectedException(typeof (System.Exception), ExpectedMessage = "Subscription ID is required")]
            public void NoSubscriptionIdRemoveTest()
            {
                SubscriptionServiceImplementation sub = new SubscriptionServiceImplementation() {_platform = Platform};
                sub.Remove();
                Thread.Sleep(500);
            }


        }
    }
}
