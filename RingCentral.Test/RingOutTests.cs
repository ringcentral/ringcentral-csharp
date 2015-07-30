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
        [TestFixtureSetUp]
        public void SetUp()
        {
            mockResponseHandler.AddPostMockResponse(
               new Uri(ApiEndPoint + RingOutEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/ringout/1?#\","+
                    "\"id\": 255,\"status\": {\"callStatus\": \"InProgress\",\"callerStatus\": \"InProgress\",\"calleeStatus\": \"InProgress\"}}", Encoding.UTF8, "application/json")
               });
            mockResponseHandler.AddGetMockResponse(
            new Uri(ApiEndPoint + RingOutEndPoint + "/1"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/ringout/1?#\"," +
                 "\"id\": 1,\"status\": {\"callStatus\": \"InProgress\",\"callerStatus\": \"InProgress\",\"calleeStatus\": \"InProgress\"}}", Encoding.UTF8, "application/json")
            });
            //TODO: Correct response once API explore is working
            mockResponseHandler.AddDeleteMockResponse(
              new Uri(ApiEndPoint + RingOutEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
        }
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

            JToken token = JObject.Parse(response.GetBody());

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