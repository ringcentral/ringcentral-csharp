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
        public void PubnubSubscription()
        {
            var subscription = sdk.CreateSubscription();
            subscription.EventFilters.Add("/restapi/v1.0/account/~/extension/~/message-store");
            subscription.EventFilters.Add("/restapi/v1.0/account/~/extension/~/presence");
            var connectCount = 0;
            subscription.ConnectEvent += (sender, args) => {
                connectCount += 1;
                Console.WriteLine(args.Message);
            };
            var messageCount = 0;
            subscription.NotificationEvent += (sender, args) => {
                messageCount += 1;
                Console.WriteLine(args.Message);
            };
            var errorCount = 0;
            subscription.ErrorEvent += (sender, args) => {
                errorCount += 1;
                Console.WriteLine(args.Message);
            };
            subscription.Register();
            SendSMS();
            Thread.Sleep(15000);
            SendSMS();
            Thread.Sleep(15000);
            subscription.Remove();
            Assert.AreEqual(1, connectCount);
            Assert.GreaterOrEqual(messageCount, 2);
            Assert.AreEqual(0, errorCount);
        }
    }
}
