using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Threading;

namespace RingCentral.Test.Real
{
    [TestFixture]
    class SubscriptionTest : BaseTest
    {
        private void SendSMS()
        {
            var requestBody = new
            {
                text = "hello world",
                from = new { phoneNumber = Config.Instance.Username },
                to = new object[] { new { phoneNumber = Config.Instance.Receiver } }
            };
            var request = new Http.Request("/restapi/v1.0/account/~/extension/~/sms", JsonConvert.SerializeObject(requestBody));
            sdk.Platform.Post(request);
        }

        [Test]
        public void MessageStoreSubscription()
        {
            var sub = new Subscription.SubscriptionServiceImplementation() { _platform = sdk.Platform };
            sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
            var count = 0;
            sub.Subscribe((message) => {
                count += 1;
                Console.WriteLine(message.ToString());
            }, null, null);

            SendSMS();
            Thread.Sleep(15000);
            SendSMS();

            Thread.Sleep(15000);
            Assert.GreaterOrEqual(count, 2);
            sub.Remove();
        }

        [Test]
        public void PresenceSubscription()
        {
            var sub = new Subscription.SubscriptionServiceImplementation() { _platform = sdk.Platform };
            sub.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
            sub.AddEvent("/restapi/v1.0/account/~/extension/~/message-store");
            var count = 0;
            sub.Subscribe((message) => {
                count += 1;
                Console.WriteLine(message.ToString());
            }, null, null);

            SendSMS();
            Thread.Sleep(15000);
            Assert.AreEqual(1, count);
            sub.Remove();
        }

        [Test]
        public void NewPubnubImplementation()
        {
            var subscription = sdk.CreateSubscription();
            subscription.EventFilters.Add("/restapi/v1.0/account/~/extension/~/message-store");
            subscription.EventFilters.Add("/restapi/v1.0/account/~/extension/~/presence");
            var connectCount = 0;
            subscription.ConnectEvent += (sender, args) => {
                connectCount += 1;
                Console.WriteLine(args.message);
            };
            var messageCount = 0;
            subscription.NotificationEvent += (sender, args) => {
                messageCount += 1;
                Console.WriteLine(args.message);
            };
            var errorCount = 0;
            subscription.ErrorEvent += (sender, args) => {
                errorCount += 1;
                Console.WriteLine(args.message);
            };
            subscription.Subscribe();
            SendSMS();
            Thread.Sleep(15000);
            subscription.Remove();
            Assert.AreEqual(1, connectCount);
            Assert.AreEqual(1, messageCount);
            Assert.AreEqual(0, errorCount);
        }
    }
}
