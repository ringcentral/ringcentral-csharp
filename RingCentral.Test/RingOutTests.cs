using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class RingOutTests : TestConfiguration
    {
        private const string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";

        [Test]
        public void CancelRingOut()
        {
            Request request = new Request(RingOutEndPoint + "/1");
            Response cancelResult = RingCentralClient.GetPlatform().Delete(request);
            Assert.AreEqual(204, cancelResult.GetStatus());
        }

        [Test]
        public void GetRingOutStatus()
        {
            Request request = new Request(RingOutEndPoint + "/1");
            Response response = RingCentralClient.GetPlatform().Get(request);

            JToken token = response.GetJson();

            var message = (string)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(message, "InProgress");
        }

        [Test]
        public void RingOut()
        {
            var jsonData = "{\"to\": {\"phoneNumber\": \"19999999999\"}," +
                                    "\"from\": {\"phoneNumber\": \"19999999999\"}," +
                                    "\"callerId\": {\"phoneNumber\": \"19999999999\"},\"playPrompt\": true}\"";
            Request request = new Request(RingOutEndPoint, jsonData);

            Response result = RingCentralClient.GetPlatform().Post(request);

            JToken token = JObject.Parse(result.GetBody());

            var callStatus = (string)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(callStatus, "InProgress");
        }
    }
}