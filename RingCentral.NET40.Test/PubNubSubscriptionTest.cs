using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace RingCentral.NET40.Test
{
    [TestFixture]
    public class PubNubSubscriptionTest : TestConfiguration
    {
        private const string SubscriptionEndPoint = "/restapi/v1.0/subscription";

        private const string JsonData =
            "{\"eventFilters\": " +
            "[ \"/restapi/v1.0/account/~/extension/~/presence\", " +
            "\"/restapi/v1.0/account/~/extension/~/message-store\" ], " +
            "\"deliveryMode\": " +
            "{ \"transportType\": \"PubNub\", \"encryption\": \"false\" } }";

        private const string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
        
        
        public Task  Wait(int milliseconds)
        {
            var tcs = new TaskCompletionSource<object>();
            new Timer(_ => tcs.SetResult(null)).Change(milliseconds, -1);
            return tcs.Task;
        }

        [Test]
        public void SetPubNubSubscription()
        {

            RingCentralClient.SetJsonData(JsonData);

            string createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var id = (string)token.SelectToken("id");

            Assert.IsNotNullOrEmpty(id);

            string getResult = RingCentralClient.GetRequest(SubscriptionEndPoint + "/" + id);

            var SubscriptionItem = JsonConvert.DeserializeObject<Subscription.Subscription>(getResult);

            Assert.IsNotNull(SubscriptionItem.DeliveryMode.Address);

            SubscriptionServiceImplementation = new Subscription.SubscriptionServiceImplementation("", SubscriptionItem.DeliveryMode.SubscriberKey);

            SubscriptionServiceImplementation.Subscribe(SubscriptionItem.DeliveryMode.Address, "", null, null, null);

            var smsHelper = new SmsHelper(toPhone, UserName, smsText);
            
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.SetJsonData(jsonObject);
            
            string result = RingCentralClient.PostRequest(SmsEndPoint);

            token = JObject.Parse(result);

            var messageStatus = (string)token.SelectToken("messageStatus");

            Assert.AreEqual(messageStatus, "Sent");

            Wait(15000).ContinueWith(_ => Debug.WriteLine("Done"));

        }
    }
}
