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

        [TestFixtureSetUp]
        public void SetUp()
        {
            mockResponseHandler.AddPostMockResponse(
              new Uri(ApiEndPoint + SubscriptionEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK) {
                  Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                    "\"/restapi/v1.0/account/1/extension/130076004/message-store\", \"/restapi/v1.0/account/1/extension/130076004/presence\" ]," +
                    "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                    "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                    "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json") 
              });
            mockResponseHandler.AddGetMockResponse(
             new Uri(ApiEndPoint + SubscriptionEndPoint + "/1"),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                   "\"/restapi/v1.0/account/1/extension/130076004/message-store\", \"/restapi/v1.0/account/1/extension/130076004/presence\" ]," +
                   "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                   "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                   "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json")
             });
            mockResponseHandler.AddDeleteMockResponse(
              new Uri(ApiEndPoint + SubscriptionEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            mockResponseHandler.AddPutMockResponse(
              new Uri(ApiEndPoint + SubscriptionEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                    "\"/restapi/v1.0/account/1/extension/130076004/message-store\", \"/restapi/v1.0/account/1/extension/130076004/presence\" ]," +
                    "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                    "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                    "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json")
              });
        }
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