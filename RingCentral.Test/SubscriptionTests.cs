using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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


        [Test]
        public void CreateSubscription()
        {
            RingCentralClient.SetJsonData(JsonData);

            var result = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(result);

            var status = (String)token.SelectToken("status");

            Assert.AreEqual(status, "Active");
        }

        [Test]
        public void GetSubscription()
        {

            RingCentralClient.SetJsonData(JsonData);

            var createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var id = (String)token.SelectToken("id");

            Assert.IsNotNullOrEmpty(id);

            var getResult = RingCentralClient.GetRequest(SubscriptionEndPoint + "/" + id);

            token = JObject.Parse(getResult);

            var getStatus = (String)token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");

        }

        [Test]
        public void RenewSubscription()
        {

            RingCentralClient.SetJsonData(JsonData);

            var createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var subscriptioniId = (String)token.SelectToken("id");

            Assert.IsNotNullOrEmpty(subscriptioniId);

            var renewResult = RingCentralClient.PutRequest(SubscriptionEndPoint + "/" + subscriptioniId);

            token = JObject.Parse(renewResult);

            var getStatus = (String)token.SelectToken("status");

            Assert.AreEqual(getStatus, "Active");

        }

        //TODO: Online API-Explorer returns an error, not sure what to expect here
        //[Test]
        public void DeleteSubscription()
        {

            RingCentralClient.SetJsonData(JsonData);

            var createResult = RingCentralClient.PostRequest(SubscriptionEndPoint);

            JToken token = JObject.Parse(createResult);

            var subscriptioniId = (String)token.SelectToken("id");

            Assert.IsNotNullOrEmpty(subscriptioniId);

            var renewResult = RingCentralClient.DeleteRequest(SubscriptionEndPoint + "/" + subscriptioniId);
        }


    }
}
