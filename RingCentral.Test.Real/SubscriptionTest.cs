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
            var sub = new Pubnub.SubscriptionServiceImplementation() { _platform = sdk.Platform };
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
            var sub = new Pubnub.SubscriptionServiceImplementation() { _platform = sdk.Platform };
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
            var sub = sdk.Platform.CreateSubscription();
            Assert.NotNull(sub);
            sub.AddEventFilter("/restapi/v1.0/account/~/extension/~/message-store");
            //sub.AddEventFilter("/restapi/v1.0/account/~/extension/~/presence");
            sub.Register();
            SendSMS();
            Thread.Sleep(1000000000);
            sub.UnRegister();
        }
    }
}
