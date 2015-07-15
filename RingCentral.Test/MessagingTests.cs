using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Helper;

namespace RingCentral.Test
{
    [TestFixture]
    public class MessagingTests : TestConfiguration
    {

        private const string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
        private const string ExtensionMessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store/";

        private const string MessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store/";

        [Test]
        public void GetSingleMessage()
        {
            const string messageStore = "1153141004";
            var result = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(result);

            var id = (String)token.SelectToken("id");

            Assert.AreEqual(messageStore,id);
        }

        [Test]
        public void UpdateMessage()
        {
            const string messageStore = "1153141004";
            var result = RingCentralClient.PutRequest(MessageEndPoint + messageStore, "{\"readStatus\": \"Read\"}");

            JToken token = JObject.Parse(result);

            var readStatus = (String)token.SelectToken("readStatus");

            Assert.AreEqual("Read", readStatus);
        }

        //The messageStore in this test needs to exist or have existed at one point otherwise it will fail
        [Test]
        public void DeleteMessage()
        {
            const string messageStore = "1153141004";
            var result = RingCentralClient.DeleteRequest(MessageEndPoint + messageStore);

            var getResult = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(getResult);

            var availability = (String)token.SelectToken("availability");

            Assert.AreEqual(availability, "Purged");
        }

        [Test]
        public void SendSms()
        {
            const string smsText = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";
            const string toPhone = "***REMOVED***"; //cellphone number of Paul

            var smsHelper = new SmsHelper(toPhone, UserName, smsText);
            var jsonObject = JsonConvert.SerializeObject(smsHelper);

            var result = RingCentralClient.PostRequest(SmsEndPoint, jsonObject);

            JToken token = JObject.Parse(result);
            var messageStatus = (String)token.SelectToken("messageStatus");

            Assert.AreEqual(messageStatus, "Sent");

        }
    }
}
