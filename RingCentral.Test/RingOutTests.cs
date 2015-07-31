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
    public class RingOutTests : TestConfiguration
    {
        private const string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";
        private const string json = "{\"to\": {\"phoneNumber\": \"19999999999\"}," +
                                    "\"from\": {\"phoneNumber\": \"19999999999\"}," +
                                    "\"callerId\": {\"phoneNumber\": \"19999999999\"},\"playPrompt\": true}\"";
    
        //TODO: this doesn't work via the online API, need to investigate
        [Test]
        public void CancelRingOut()
        {

            Response cancelResult = RingCentralClient.GetPlatform().DeleteRequest(RingOutEndPoint + "/1");
            Assert.AreEqual(204, cancelResult.GetStatus());
        }

        [Test]
        public void GetRingOutStatus()
        {
            
            Response response = RingCentralClient.GetPlatform().GetRequest(RingOutEndPoint + "/1");

            JToken token = response.GetJson();

            var message = (string) token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(message, "InProgress");
        }

        [Test]
        public void RingOut()
        {
            RingCentralClient.GetPlatform().SetJsonData(json);

            Response result = RingCentralClient.GetPlatform().PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result.GetBody());

            var callStatus = (string) token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(callStatus, "InProgress");
        }
    }
}