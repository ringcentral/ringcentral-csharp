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

        [Test]
        public void DeleteSubscription()
        {
            Request request = new Request(SubscriptionEndPoint + "/1");
            Response result = RingCentralClient.GetPlatform().Delete(request);
            Assert.AreEqual(204, result.GetStatus());
        }


        [Test]
        public void CreateSubscription()
        {
            var jsonData = "{\"eventFilters\": " +
            "[ \"/restapi/v1.0/account/~/extension/~/presence\", " +
            "\"/restapi/v1.0/account/~/extension/~/message-store\" ], " +
            "\"deliveryMode\": { \"transportType\": \"PubNub\", \"encryption\": \"false\" } }";

            Request request = new Request(SubscriptionEndPoint, jsonData);
            Response result = RingCentralClient.GetPlatform().Post(request);

            JToken token = JObject.Parse(result.GetBody());

            var status = (string)token.SelectToken("status");

            Assert.AreEqual(status, "Active");
        }

        [Test]
        public void GetSubscription()
        {
            Request request = new Request(SubscriptionEndPoint + "/1");
            Response response = RingCentralClient.GetPlatform().Get(request);

            var subscriptionItem = JsonConvert.DeserializeObject<Subscription.Subscription>(response.GetBody());

            Assert.AreEqual(subscriptionItem.DeliveryMode.TransportType, "PubNub");

            Assert.AreEqual(subscriptionItem.DeliveryMode.Encryption, true);

            Assert.IsNotEmpty(subscriptionItem.EventFilters);

            Assert.AreEqual(subscriptionItem.Status, "Active");
        }

        [Test]
        public void RenewSubscription()
        {
            var jsonData = "{\"eventFilters\": [\"/restapi/v1.0/account/~/extension/~/presence\"," +
                           "\"/restapi/v1.0/account/~/extension/~/message-store\" ]}";

            Request request = new Request(SubscriptionEndPoint + "/1", jsonData);
            Response renewResult = RingCentralClient.GetPlatform().Put(request);

            JToken token = renewResult.GetJson();

            var getStatus = (string)token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");
        }
    }
}