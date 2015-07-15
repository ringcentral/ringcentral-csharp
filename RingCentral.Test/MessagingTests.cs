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
            var result = RingCentralClient.PutRequest(ExtensionMessageEndPoint + messageStore, "{\"readStatus\": \"Read\"}");

            JToken token = JObject.Parse(result);

            var readStatus = (String)token.SelectToken("readStatus");

            Assert.AreEqual("Read", readStatus);
        }

        //The messageStore in this test needs to exist or have existed at one point otherwise it will fail
        [Test]
        public void DeleteMessage()
        {
            const string messageStore = "1153141004";
            var result = RingCentralClient.DeleteRequest(ExtensionMessageEndPoint + messageStore);

            var getResult = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(getResult);

            var availability = (String)token.SelectToken("availability");

            Assert.AreEqual(availability, "Purged");
        }
        //TODO: need to find a valid conversationId to delete to pass this test
        [Test]
        public void DeleteConversationById()
        {
            const string conversationId = "1035491849837189800";

            var result = RingCentralClient.DeleteRequest(ExtensionMessageEndPoint + conversationId);
            var getResult = RingCentralClient.GetRequest(ExtensionMessageEndPoint + conversationId);

        }

        [Test]
        public void GetMessagesFromExtension()
        {
            var result = RingCentralClient.GetRequest(ExtensionMessageEndPoint);

            JToken token = JObject.Parse(result);
            var phoneNumber = (String)token.SelectToken("records")[0].SelectToken("from").SelectToken("phoneNumber");

            Assert.AreEqual(phoneNumber.Replace("+",String.Empty), UserName);

        }

        [Test]
        public void GetAttachmentFromExtension()
        {
            const string messageId = "1152989004";
            const string contentId = "1152989004";
            const string expectedMessage = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

            var result = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageId + "/content/" + contentId);

            Assert.AreEqual(result, expectedMessage);

        }

        [Test]
        public void SendFax()
        {
            //TODO: unimplemented
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

        [Test]
        public void SendPagerMessage()
        {
            //TODO unimplemented
        }
    }
}
