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
    public class PresenceTests : TestConfiguration
    {
        private const string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";
        [TestFixtureSetUp]
        public void SetUp()
        {
            mockResponseHandler.AddGetMockResponse(
               new Uri(ApiEndPoint + PresenceEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/presence\"," +
                    "\"extension\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," +
                    "\"id\": 130076004,\"extensionNumber\": \"101\"}," +
                    "\"presenceStatus\": \"Available\",\"telephonyStatus\": \"NoCall\",\"userStatus\": \"Available\"," +
                    "\"dndStatus\": \"TakeAllCalls\",\"allowSeeMyPresence\": true,\"ringOnMonitoredCall\": false,\"pickUpCallsOnHold\": false}"
                    , Encoding.UTF8, "application/json")
               });
        }
        [Test]
        public void GetPresence()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(PresenceEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var presenceStatus = (string) token.SelectToken("presenceStatus");

            Assert.AreEqual(presenceStatus, "Available");
        }
    }
}