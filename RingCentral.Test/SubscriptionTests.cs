using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

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
            RingCentralClient.GetPlatform().SetJsonData(JsonData);

            Response createResult = RingCentralClient.GetPlatform().PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult.GetBody());

            var subscriptioniId = (string) token.SelectToken("id");

            Assert.IsNotNullOrEmpty(subscriptioniId);

            Response renewResult = RingCentralClient.GetPlatform().DeleteRequest(SubscriptionEndPoint + "/" + subscriptioniId);
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
            RingCentralClient.GetPlatform().SetJsonData(JsonData);

            Response createResult = RingCentralClient.GetPlatform().PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult.GetBody());

            var id = (string) token.SelectToken("id");

            Assert.IsNotNullOrEmpty(id);

            Response response = RingCentralClient.GetPlatform().GetRequest(SubscriptionEndPoint + "/" + id);

            var SubscriptionItem = JsonConvert.DeserializeObject<Subscription.Subscription>(response.GetBody());

            Assert.AreEqual(SubscriptionItem.DeliveryMode.TransportType, "PubNub");

            Assert.AreEqual(SubscriptionItem.DeliveryMode.Encryption, true);

            Assert.IsNotEmpty(SubscriptionItem.EventFilters);

            Assert.AreEqual(SubscriptionItem.Status, "Active");
        }

        [Test]
        public void RenewSubscription()
        {
            RingCentralClient.GetPlatform().SetJsonData(JsonData);

            Response createResult = RingCentralClient.GetPlatform().PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult.GetBody());

            var subscriptioniId = (string) token.SelectToken("id");

            Assert.IsNotNullOrEmpty(subscriptioniId);

            Response renewResult = RingCentralClient.GetPlatform().PutRequest(SubscriptionEndPoint + "/" + subscriptioniId);

            token = renewResult.GetJson();

            var getStatus = (string) token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");
        }
    }
}