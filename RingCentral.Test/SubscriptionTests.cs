using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class SubscriptionTests : TestConfiguration
    {
        private const string SubscriptionEndPoint = "/restapi/v1.0/subscription";

        private const string JsonData =
            "{\"eventFilters\": " +
            "[ \"/restapi/v1.0/account/~/extension/~/presence\", " +
            "\"/restapi/v1.0/account/~/extension/~/message-store\" ], " +
            "\"deliveryMode\": " +
            "{ \"transportType\": \"PubNub\", \"encryption\": \"false\" } }";

 
        public void DeleteSubscription()
        {
            Request request = new Request(SubscriptionEndPoint + "/1");
            Response result = RingCentralClient.GetPlatform().DeleteRequest(request);
            Assert.AreEqual(204, result.GetStatus());
        }


        [Test]
        public void CreateSubscription()
        {
            Request request = new Request(SubscriptionEndPoint,JsonData);
            Response result = RingCentralClient.GetPlatform().PostRequest(request);

            JToken token = JObject.Parse(result.GetBody());

            var status = (string) token.SelectToken("status");

            Assert.AreEqual(status, "Active");
        }

        [Test]
        public void GetSubscription()
        {
            Request request = new Request(SubscriptionEndPoint + "/1");
            Response response = RingCentralClient.GetPlatform().GetRequest(request);

            var SubscriptionItem = JsonConvert.DeserializeObject<Subscription.Subscription>(response.GetBody());

            Assert.AreEqual(SubscriptionItem.DeliveryMode.TransportType, "PubNub");

            Assert.AreEqual(SubscriptionItem.DeliveryMode.Encryption, true);

            Assert.IsNotEmpty(SubscriptionItem.EventFilters);

            Assert.AreEqual(SubscriptionItem.Status, "Active");
        }
        //TODO: need to add json body
        //[Test]
        public void RenewSubscription()
        {
            Request request = new Request(SubscriptionEndPoint + "/1");
            Response renewResult = RingCentralClient.GetPlatform().PutRequest(request);

            JToken token = renewResult.GetJson();

            var getStatus = (string) token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");
        }
    }
}