using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class MessagingTest : BaseTest
    {
        private const string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
        private const string ExtensionMessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store";
        private const string FaxEndPoint = "/restapi/v1.0/account/~/extension/~/fax";
        private const string PagerEndPoint = "/restapi/v1.0/account/~/extension/~/company-pager";

        private readonly string[] _messageSentValues = { "Sent", "Queued" };



        [Test]
        public void DeleteConversationById()
        {
            Request request = new Request(ExtensionMessageEndPoint + "/123123123");
            ApiResponse result = sdk.Platform.Delete(request);
            Assert.AreEqual(204, result.GetStatus());

        }

        [Test]
        public void DeleteMessage()
        {
            Request request = new Request(ExtensionMessageEndPoint + "/123");
            ApiResponse result = sdk.Platform.Delete(request);
            Assert.AreEqual(204, result.GetStatus());

        }


        [Test]
        public void GetAttachmentFromExtension()
        {
            const string expectedMessage = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

            Request request = new Request(ExtensionMessageEndPoint + "/1/content/1");
            ApiResponse response = sdk.Platform.Get(request);

            Assert.AreEqual(response.GetBody(), expectedMessage);
        }


        [Test]
        public void GetBatchMessage()
        {
            var messages = new List<string> { "1", "2" };
            string batchMessages = messages.Aggregate("", (current, message) => current + (message + ","));

            Request request = new Request(ExtensionMessageEndPoint + "/" + batchMessages);
            ApiResponse response = sdk.Platform.Get(request);
            Assert.IsTrue(response.IsMultiPartResponse());
            List<string> multiPartResult = response.GetMultiPartResponses();

            //We're interested in the response statuses and making sure the result was ok for each of the message id's sent.
            JToken responseStatuses = JObject.Parse(multiPartResult[0]);
            for (int i = 0; i < messages.Count; i++)
            {
                var status = (string)responseStatuses.SelectToken("response")[i].SelectToken("status");
                Assert.AreEqual(status, "200");
            }

            foreach (string res in multiPartResult.Skip(1))
            {
                JToken token = JObject.Parse(res);
                var id = (string)token.SelectToken("id");
                Assert.IsNotNull(id);
            }

        }

        [Test]
        public void GetMessagesFromExtension()
        {
            Request request = new Request(ExtensionMessageEndPoint);
            ApiResponse response = sdk.Platform.Get(request);

            JToken token = response.GetJson();
            var phoneNumber = (string)token.SelectToken("records")[0].SelectToken("from").SelectToken("phoneNumber");

            Assert.AreEqual("+19999999999", phoneNumber);
        }

        [Test]
        public void GetSingleMessage()
        {
            Request request = new Request(ExtensionMessageEndPoint + "/2");
            ApiResponse response = sdk.Platform.Get(request);

            JToken token = response.GetJson();

            var id = (string)token.SelectToken("id");

            Assert.AreEqual("2", id);
        }


        [Test]
        public void SendFax()
        {
            const string text = "Hello world!";
            var json = "{\"to\":[{\"phoneNumber\":\"258369741\"}],\"faxResolution\":\"High\"}";

            var byteArrayText = Encoding.UTF8.GetBytes(text);

            var attachment = new Attachment("test.txt", "application/octet-stream", byteArrayText);
            var attachment2 = new Attachment("test2.txt", "text/plain", byteArrayText);

            var attachments = new List<Attachment> { attachment, attachment2 };

            Request request = new Request(FaxEndPoint, json, attachments);
            ApiResponse response = sdk.Platform.Post(request);

            JToken token = response.GetJson();
            var availability = (string)token.SelectToken("availability");

            Assert.AreEqual("Alive", availability);
        }


        [Test]
        public void SendPagerMessage()
        {
            var from = "101";
            var to1 = "102";
            var to2 = "103";
            var text = "Hello";
            var jsonData = "{\"to\": [{\"extensionNumber\": \"" + to1 + "\"}, {\"extensionNumber\": \"" + to2 + "\"}]," +
                            "\"from\": {\"extensionNumber\": \"" + from + "\"},\"text\": \"" + text + "\"}";
            Request request = new Request(PagerEndPoint, jsonData);
            ApiResponse result = sdk.Platform.Post(request);
            JToken token = result.GetJson();
            var id = (int)token.SelectToken("id");
            Assert.AreEqual(8, id);
        }

        [Test]
        public void SendSms()
        {
            var from = "19999999999";
            var to = "19999999999";
            var text = "Test SMS message";
            var jsonData = "{\"to\": [{\"phoneNumber\": \"" + to + "\"}]," +
                           "\"from\": {\"phoneNumber\": \"" + from + "\"}," +
                          "\"text\": \"" + text + "\" }";
            Request request = new Request(SmsEndPoint, jsonData);
            ApiResponse result = sdk.Platform.Post(request);

            JToken token = JObject.Parse(result.GetBody());
            var messageStatus = (string)token.SelectToken("messageStatus");
            Assert.Contains(messageStatus, _messageSentValues);
        }


        [Test]
        public void UpdateMessage()
        {
            var jsonData = "{\"readStatus\": \"Read\"}";
            Request request = new Request(ExtensionMessageEndPoint + "/3", jsonData);
            ApiResponse result = sdk.Platform.Put(request);
            JToken token = JObject.Parse(result.GetBody());
            var availability = (string)token.SelectToken("availability");
            var readStatus = (string)token.SelectToken("readStatus");
            Assert.AreEqual("Read", readStatus);

        }
    }
}