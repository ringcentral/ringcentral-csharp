﻿using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class RingOutTest : BaseTest
    {
        private const string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";

        [Test]
        public void CancelRingOut()
        {
            Request request = new Request(RingOutEndPoint + "/1");
            ApiResponse cancelResult = sdk.Platform.Delete(request);
            Assert.AreEqual(204, cancelResult.Status);
        }

        [Test]
        public void GetRingOutStatus()
        {
            Request request = new Request(RingOutEndPoint + "/1");
            ApiResponse response = sdk.Platform.Get(request);

            JToken token = response.Json;

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

            ApiResponse result = sdk.Platform.Post(request);

            JToken token = JObject.Parse(result.Body);

            var callStatus = (string)token.SelectToken("status").SelectToken("callStatus");

            Assert.AreEqual(callStatus, "InProgress");
        }
    }
}