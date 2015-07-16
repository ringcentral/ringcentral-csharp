using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class RingOutTests : TestConfiguration
    {
        private const string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";

        [Test]
        public void RingOut()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.SetJsonData(json);

            var result = RingCentralClient.PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result);

            var callStatus = (String)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(callStatus, "InProgress");
        }

        [Test]
        public void GetRingOutStatus()
        {

            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.SetJsonData(json);

            var result = RingCentralClient.PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result);

            var id = (String)token.SelectToken("id");

            Assert.IsNotNull(id);

            var getStatusResult = RingCentralClient.GetRequest(RingOutEndPoint + "/"  + id);

            token = JObject.Parse(getStatusResult);

            var message = (String)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(message,"InProgress");
        }

        //TODO: this doesn't work via the online API, need to investigate
        [Test]
        public void CancelRingOut()
        {
            const string json = "{\"to\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"from\": {\"phoneNumber\": \"***REMOVED***\"}," +
                                "\"callerId\": {\"phoneNumber\": \"***REMOVED***\"},\"playPrompt\": true}\"";

            RingCentralClient.SetJsonData(json);

            var result = RingCentralClient.PostRequest(RingOutEndPoint);

            JToken token = JObject.Parse(result);

            var id = (String) token.SelectToken("id");

            Assert.IsNotNull(id);

            var cancelResult = RingCentralClient.DeleteRequest(RingOutEndPoint + "/" + id);

            
        }
    }
}
