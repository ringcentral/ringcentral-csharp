using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

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
            RingCentralClient.SetJsonData(JsonData);

            string createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var subscriptioniId = (string) token.SelectToken("id");

            Assert.IsNotNullOrEmpty(subscriptioniId);

            string renewResult = RingCentralClient.DeleteRequest(SubscriptionEndPoint + "/" + subscriptioniId);
        }


        [Test]
        public void CreateSubscription()
        {
            RingCentralClient.SetJsonData(JsonData);

            string result = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(result);

            var status = (string) token.SelectToken("status");

            Assert.AreEqual(status, "Active");
        }

        [Test]
        public void GetSubscription()
        {
            RingCentralClient.SetJsonData(JsonData);

            string createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var id = (string) token.SelectToken("id");

            Assert.IsNotNullOrEmpty(id);

            string getResult = RingCentralClient.GetRequest(SubscriptionEndPoint + "/" + id);

            token = JObject.Parse(getResult);

            var getStatus = (string) token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");
        }

        [Test]
        public void RenewSubscription()
        {
            RingCentralClient.SetJsonData(JsonData);

            string createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var subscriptioniId = (string) token.SelectToken("id");

            Assert.IsNotNullOrEmpty(subscriptioniId);

            string renewResult = RingCentralClient.PutRequest(SubscriptionEndPoint + "/" + subscriptioniId);

            token = JObject.Parse(renewResult);

            var getStatus = (string) token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");
        }
    }
}