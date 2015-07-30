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
            //TODO: Get proper result once API explore is fixed
            Response result = RingCentralClient.GetPlatform().DeleteRequest(SubscriptionEndPoint + "/1");
            Assert.AreEqual(204, result.GetStatus());
        }


        [Test]
        public void CreateSubscription()
        {
            RingCentralClient.GetPlatform().SetJsonData(JsonData);

            Response result = RingCentralClient.GetPlatform().PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(result.GetBody());

            var status = (string) token.SelectToken("status");

            Assert.AreEqual(status, "Active");
        }

        [Test]
        public void GetSubscription()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(SubscriptionEndPoint + "/1");

            var SubscriptionItem = JsonConvert.DeserializeObject<Subscription.Subscription>(response.GetBody());

            Assert.AreEqual(SubscriptionItem.DeliveryMode.TransportType, "PubNub");

            Assert.AreEqual(SubscriptionItem.DeliveryMode.Encryption, true);

            Assert.IsNotEmpty(SubscriptionItem.EventFilters);

            Assert.AreEqual(SubscriptionItem.Status, "Active");
        }

        [Test]
        public void RenewSubscription()
        {
           
            Response renewResult = RingCentralClient.GetPlatform().PutRequest(SubscriptionEndPoint + "/1");

            JToken token = renewResult.GetJson();

            var getStatus = (string) token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");
        }
    }
}