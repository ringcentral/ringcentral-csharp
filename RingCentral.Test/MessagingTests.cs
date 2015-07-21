using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        //TODO: need to find a valid conversationId to delete to pass this test
        [Test]
        public void DeleteConversationById()
        {
            const string conversationId = "1035491849837189800";

            string result = RingCentralClient.DeleteRequest(ExtensionMessageEndPoint + conversationId);
            string getResult = RingCentralClient.GetRequest(ExtensionMessageEndPoint + conversationId);
        }

        [Test]
        public void DeleteMessage()
        {
            const string messageStore = "1153141004";
            string result = RingCentralClient.DeleteRequest(ExtensionMessageEndPoint + messageStore);

            string getResult = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(getResult);

            var availability = (string) token.SelectToken("availability");

            Assert.AreEqual(availability, "Purged");
        }

        [Test]
        public void GetAttachmentFromExtension()
        {
            const string messageId = "1152989004";
            const string contentId = "1152989004";
            const string expectedMessage = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

            string result = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageId + "/content/" + contentId);

            Assert.AreEqual(result, expectedMessage);
        }

        [Test]
        public void GetMessagesFromExtension()
        {
            string result = RingCentralClient.GetRequest(ExtensionMessageEndPoint);

            JToken token = JObject.Parse(result);
            var phoneNumber = (string) token.SelectToken("records")[0].SelectToken("from").SelectToken("phoneNumber");

            Assert.AreEqual(phoneNumber.Replace("+", string.Empty), UserName);
        }

        [Test]
        public void GetSingleMessage()
        {
            const string messageStore = "1153141004";
            string result = RingCentralClient.GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(result);

            var id = (string) token.SelectToken("id");

            Assert.AreEqual(messageStore, id);
        }

        [Test]
        public void GetBatchMessage()
        {
            var messages = new List<string> {"1180709004", "1180693004"};
            var batchMessages = messages.Aggregate("", (current, message) => current + (message + ","));

            string result = RingCentralClient.GetRequest(ExtensionMessageEndPoint + batchMessages);

            if (RingCentralClient.IsMultiPartResponse)
            {
                var multiPartResult = RingCentralClient.GetMultiPartResponses(result);

                //We're interested in the response statuses and making sure the result was ok for each of the message id's sent.
                JToken responseStatuses = JObject.Parse(multiPartResult[0]);
                for (int i = 0; i < messages.Count; i++)
                {
                    string status = (string)responseStatuses.SelectToken("response")[i].SelectToken("status");
                    Assert.AreEqual(status,"200");
                }

                foreach (var response in multiPartResult.Skip(1))
                {
                    JToken token = JObject.Parse(response);
                    string id = (string) token.SelectToken("id");
                    Assert.IsNotNull(id);
                }
            }
        }

        [Test]
        public void SendFax()
        {
            //TODO: unimplemented
        }

        [Test]
        public void SendPagerMessage()
        {
            //TODO unimplemented
        }

        [Test]
        public void SendSms()
        {
            const string smsText = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";
            //const string toPhone = "***REMOVED***"; //cellphone number of Paul
            const string toPhone = "***REMOVED***"; //cellphone number of Nate
            var smsHelper = new SmsHelper(toPhone, UserName, smsText);
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.SetJsonData(jsonObject);

            string result = RingCentralClient.PostRequest(SmsEndPoint);

            JToken token = JObject.Parse(result);
            var messageStatus = (string) token.SelectToken("messageStatus");

            Assert.AreEqual(messageStatus, "Sent");
        }

        [Test]
        public void UpdateMessage()
        {
            const string messageStore = "1153141004";

            RingCentralClient.SetJsonData("{\"readStatus\": \"Read\"}");

            string result = RingCentralClient.PutRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(result);

            var availability = (string) token.SelectToken("availability");

            if (availability.Equals("Purged"))
            {
                Assert.AreEqual("Purged", availability);
            }
            else
            {
                var readStatus = (string) token.SelectToken("readStatus");
                Assert.AreEqual("Read", readStatus);
            }
        }
    }
}