using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RingCentral.Test.Real
{
    [TestFixture]
    class SubscriptionTest : BaseTest
    {
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

            var dict = new Dictionary<string, dynamic> {
                { "text", "hello world" },
                { "from", new Dictionary<string, string> { { "phoneNumber", Config.Instance.Username} } },
                { "to", new Dictionary<string, string>[] { new Dictionary<string, string> { { "phoneNumber", Config.Instance.Receiver } } } },
            };
            var request = new Http.Request("/restapi/v1.0/account/~/extension/~/sms", JsonConvert.SerializeObject(dict));
            sdk.Platform.Post(request);
            Thread.Sleep(15000);
            sdk.Platform.Post(request);

            Thread.Sleep(15000);
            Assert.AreEqual(2, count);
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

            var dict = new Dictionary<string, dynamic> {
                { "text", "hello world" },
                { "from", new Dictionary<string, string> { { "phoneNumber", Config.Instance.Username} } },
                { "to", new Dictionary<string, string>[] { new Dictionary<string, string> { { "phoneNumber", Config.Instance.Receiver } } } },
            };
            var request = new Http.Request("/restapi/v1.0/account/~/extension/~/sms", JsonConvert.SerializeObject(dict));
            sdk.Platform.Post(request);
            Thread.Sleep(15000);
            Assert.AreEqual(1, count);
            sub.Remove();
        }
    }
}
