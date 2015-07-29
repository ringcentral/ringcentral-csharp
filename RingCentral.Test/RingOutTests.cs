using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class RingOutTests : TestConfiguration
    {
        private const string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";

        //TODO: this doesn't work via the online API, need to investigate
        [Test]
        public void CancelRingOut()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.GetPlatform().SetJsonData(json);

            Response result = RingCentralClient.GetPlatform().PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result.GetBody());

            var id = (string) token.SelectToken("id");

            Assert.IsNotNull(id);

            Response cancelResult = RingCentralClient.GetPlatform().DeleteRequest(RingOutEndPoint + "/" + id);
        }

        [Test]
        public void GetRingOutStatus()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.GetPlatform().SetJsonData(json);

            Response result = RingCentralClient.GetPlatform().PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result.GetBody());

            var id = (string) token.SelectToken("id");

            Assert.IsNotNull(id);

            Response response = RingCentralClient.GetPlatform().GetRequest(RingOutEndPoint + "/" + id);

            token = JObject.Parse(response.GetBody());

            var message = (string) token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(message, "InProgress");
        }

        [Test]
        public void RingOut()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.GetPlatform().SetJsonData(json);

            Response result = RingCentralClient.GetPlatform().PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result.GetBody());

            var callStatus = (string) token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(callStatus, "InProgress");
        }
    }
}