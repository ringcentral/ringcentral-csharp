using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentralSDK.Test
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

            RingCentralClient.SetJsonData(json);

            string result = RingCentralClient.PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result);

            var id = (string)token.SelectToken("id");

            Assert.IsNotNull(id);

            string cancelResult = RingCentralClient.DeleteRequest(RingOutEndPoint + "/" + id);
        }

        [Test]
        public void GetRingOutStatus()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.SetJsonData(json);

            string result = RingCentralClient.PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result);

            var id = (string)token.SelectToken("id");

            Assert.IsNotNull(id);

            string getStatusResult = RingCentralClient.GetRequest(RingOutEndPoint + "/" + id);

            token = JObject.Parse(getStatusResult);

            var message = (string)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(message, "InProgress");
        }

        [Test]
        public void RingOut()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.SetJsonData(json);

            string result = RingCentralClient.PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result);

            var callStatus = (string)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(callStatus, "InProgress");
        }
    }
}
